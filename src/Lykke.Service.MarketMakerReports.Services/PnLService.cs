using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots;
using Lykke.Service.MarketMakerReports.Core.Domain.PnL;
using Lykke.Service.MarketMakerReports.Core.Exceptions;
using Lykke.Service.MarketMakerReports.Core.Services;
using Lykke.Service.NettingEngine.Client;

namespace Lykke.Service.MarketMakerReports.Services
{
    public class PnLService : IPnLService
    {
        private readonly IInventorySnapshotService _inventorySnapshotService;
        private readonly INettingEngineServiceClient _nettingEngineServiceClient;

        private const string LykkeExchangeName = "lykke";

        public PnLService(IInventorySnapshotService inventorySnapshotService,
            INettingEngineServiceClient nettingEngineServiceClient)
        {
            _inventorySnapshotService = inventorySnapshotService;
            _nettingEngineServiceClient = nettingEngineServiceClient;
        }

        public async Task<PnLResult> CalculatePnLAsync(DateTime startDate, DateTime endDate)
        {
            var startSnapshot = await GetInventorySnapshot(startDate) 
                                ?? throw new InvalidPnLCalculationException("No InventorySnapshot for startDate");
            
            var endSnapshot = await GetInventorySnapshot(endDate) 
                              ?? throw new InvalidPnLCalculationException("No InventorySnapshot for startDate");

            var depositChanges = await GetChangeDepositOperations(startDate, endDate);

            var pnLs = startSnapshot.Assets.Join(endSnapshot.Assets, x => x.Asset, x => x.Asset,
                (start, end) => CalcPnLForAsset(start.Asset, start, end, 
                    depositChanges.SingleOrDefault(d => string.Equals(d.asset, start.Asset, StringComparison.InvariantCultureIgnoreCase)).depositChanges));

            var (adjusted, directional) = SumByColumns(pnLs.Select(x => (x.adjustedPnL, x.directionalPnL)));

            return new PnLResult(adjusted, directional);
        }

        private async Task<IEnumerable<(string asset, decimal depositChanges)>> GetChangeDepositOperations(DateTime startDate, DateTime endDate)
        {
            var operations = await _nettingEngineServiceClient.ChangeDepositOperationApi.GetAsync(startDate, endDate, LykkeExchangeName,
                null);

            return operations.GroupBy(x => x.AssetId).Select(x => (asset: x.Key, depositChanges: x.Sum(o => o.Amount)));
        }
        
        private (decimal, decimal) SumByColumns(IEnumerable<(decimal, decimal)> enumerable)
        {
            var sum1 = 0m;
            var sum2 = 0m;
            
            foreach (var tuple in enumerable)
            {
                sum1 += tuple.Item1;
                sum2 += tuple.Item2;
            }

            return (sum1, sum2);
        }

        private Task<InventorySnapshot> GetInventorySnapshot(DateTime dateTime)
        {
            return _inventorySnapshotService.GetAsync(dateTime);
        }

        private (string asset, decimal adjustedPnL, decimal directionalPnL) CalcPnLForAsset(string asset,
            AssetBalanceInventory start, AssetBalanceInventory end,
            decimal depositChanges)
        {
            var startBalanceAndPrice = GetBalanceAndPriceOnLykke(start);
            var endBalanceAndPrice = GetBalanceAndPriceOnLykke(end);

            var inventory = endBalanceAndPrice.balanceOnLykke - startBalanceAndPrice.balanceOnLykke;
            var adjustedPnL = endBalanceAndPrice.priceInUsd * (inventory - depositChanges);
            var directionalPnL = startBalanceAndPrice.balanceOnLykke * (endBalanceAndPrice.priceInUsd - startBalanceAndPrice.priceInUsd);

            return (asset, adjustedPnL, directionalPnL);
        }

        private (decimal balanceOnLykke, decimal priceInUsd) GetBalanceAndPriceOnLykke(
            AssetBalanceInventory assetBalanceInventory)
        {
            var balance = assetBalanceInventory.Balances.SingleOrDefault(x =>
                string.Equals(x.Exchange, LykkeExchangeName, StringComparison.InvariantCultureIgnoreCase));

            return balance == null ? (0, 0) : 
                (balance.Amount, balance.AmountInUsd != 0 ? balance.Amount / balance.AmountInUsd : 0);
        }
    }
}
