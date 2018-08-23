using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots;
using Lykke.Service.MarketMakerReports.Core.Domain.PnL;
using Lykke.Service.MarketMakerReports.Core.Exceptions;
using Lykke.Service.MarketMakerReports.Core.Services;
using Lykke.Service.NettingEngine.Client;
using Lykke.Service.NettingEngine.Client.Models.Inventories;

namespace Lykke.Service.MarketMakerReports.Services
{
    public class PnLService : IPnLService
    {
        private readonly IInventorySnapshotService _inventorySnapshotService;
        private readonly INettingEngineServiceClient _nettingEngineServiceClient;
        private readonly ILog _log;

        private const string LykkeExchangeName = "lykke";

        public PnLService(
            ILogFactory logFactory,
            IInventorySnapshotService inventorySnapshotService,
            INettingEngineServiceClient nettingEngineServiceClient)
        {
            _log = logFactory.CreateLog(this);
            _inventorySnapshotService = inventorySnapshotService;
            _nettingEngineServiceClient = nettingEngineServiceClient;
        }

        public async Task<PnLResult> GetPnLAsync(DateTime startDate, DateTime endDate)
        {
            (InventorySnapshot startSnapshot, InventorySnapshot endSnapshot) = await _inventorySnapshotService.GetStartEndSnapshotsAsync(startDate, endDate);
            
            if (startSnapshot == null)
            {
                throw new InvalidPnLCalculationException($"No InventorySnapshot for startDate {startDate}");
            }

            if (endSnapshot == null)
            {
                throw new InvalidPnLCalculationException($"No InventorySnapshot for endDate {endDate}");
            }

            IDictionary<string, decimal> depositChanges = await GetChangeDepositOperations(startDate, endDate);

            
            // get all assets from Start snapshot, all assets from End snapshot, join them to 
            // pairs (startSnapshot, endSnapshot) per asset and calculate PnL for every asset.
            var pnLs = startSnapshot.Assets.Join(endSnapshot.Assets, 
                    x => x.AssetId, x => x.AssetId,
                    (start, end) => CalcPnLForAsset(start.AssetDisplayId, start, end, depositChanges.ContainsKey(start.AssetId) ? depositChanges[start.AssetId] : 0))
                .Where(x => !x.IsEmpty())
                .ToList();

            return new PnLResult
                {
                    Adjusted = pnLs.Sum(x => x.Adjusted),
                    Directional = pnLs.Sum(x => x.Directional),
                    StartDate = startSnapshot.Timestamp,
                    EndDate = endSnapshot.Timestamp,
                    AssetsPnLs = pnLs
                };
        }

        public Task<PnLResult> GetCurrentDayPnLAsync()
        {
            var now = DateTime.UtcNow;
            return GetPnLAsync(now.Date, now);
        }

        public Task<PnLResult> GetCurrentMonthPnLAsync()
        {
            var now = DateTime.UtcNow;
            return GetPnLAsync(now.RoundToMonth(), now);
        }

        private async Task<IDictionary<string, decimal>> GetChangeDepositOperations(DateTime startDate, DateTime endDate)
        {
            IReadOnlyList<ChangeDepositOperationSumModel> operations = await _nettingEngineServiceClient.ChangeDepositOperationApi
                .GetSumsAsync(startDate, endDate, LykkeExchangeName, null);

            if (operations == null)
            {
                throw new InvalidPnLCalculationException("NettingEngine returned null for ChangeDepositOperations");
            }

            return operations.ToDictionary(x => x.AssetId, x => x.SumOfAllOperations);
        }

        private AssetPnL CalcPnLForAsset(string asset,
            AssetBalanceInventory start, 
            AssetBalanceInventory end,
            decimal depositChanges)
        {
            var startAssetBalance = new BalanceOnDate(start.GetBalanceForExchange(LykkeExchangeName));
            var endAssetBalance = new BalanceOnDate(end.GetBalanceForExchange(LykkeExchangeName));

            var inventory = endAssetBalance.Balance - startAssetBalance.Balance;
            var adjustedPnL = endAssetBalance.Price * (inventory - depositChanges);
            var directionalPnL = startAssetBalance.Balance * (endAssetBalance.Price - startAssetBalance.Price);

            return new AssetPnL
                {
                    Asset = asset,
                    Adjusted = adjustedPnL,
                    Directional = directionalPnL,
                    StartBalance = startAssetBalance,
                    EndBalance = endAssetBalance,
                    SumOfChangeDepositOperations = depositChanges
                };
        }
    }
}
