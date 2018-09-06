using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
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

        public PnLService(
            IInventorySnapshotService inventorySnapshotService,
            INettingEngineServiceClient nettingEngineServiceClient)
        {
            _inventorySnapshotService = inventorySnapshotService;
            _nettingEngineServiceClient = nettingEngineServiceClient;
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

        public async Task<PnLResult> GetPnLAsync(DateTime startDate, DateTime endDate)
        {
            (InventorySnapshot startSnapshot, InventorySnapshot endSnapshot) = 
                await _inventorySnapshotService.GetStartEndSnapshotsAsync(startDate, endDate);
            
            if (startSnapshot == null)
            {
                throw new InvalidPnLCalculationException($"No InventorySnapshot for startDate {startDate}");
            }

            if (endSnapshot == null)
            {
                throw new InvalidPnLCalculationException($"No InventorySnapshot for endDate {endDate}");
            }

            var depositChanges = await GetChangeDepositOperations(startDate, endDate);

            return new PnLCalculator().GetPnL(startSnapshot, endSnapshot, depositChanges);
        }

        private async Task<IReadOnlyList<ChangeDepositOperationSumModel>> GetChangeDepositOperations(DateTime startDate, DateTime endDate)
        {
            IReadOnlyList<ChangeDepositOperationSumModel> operations = await _nettingEngineServiceClient.ChangeDepositOperationApi
                .GetSumsAsync(startDate, endDate, null, null);

            if (operations == null)
            {
                throw new InvalidPnLCalculationException("NettingEngine returned null for ChangeDepositOperations");
            }

            return operations;
        }
    }
}
