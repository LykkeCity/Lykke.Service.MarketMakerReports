using System;
using System.Threading.Tasks;
using Common;
using Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots;
using Lykke.Service.MarketMakerReports.Core.Domain.PnL;
using Lykke.Service.MarketMakerReports.Core.Exceptions;
using Lykke.Service.MarketMakerReports.Core.Services;

namespace Lykke.Service.MarketMakerReports.Services.TradingPnL
{
    public class PnLService : IPnLService
    {
        private readonly IInventorySnapshotService _inventorySnapshotService;

        public PnLService(IInventorySnapshotService inventorySnapshotService)
        {
            _inventorySnapshotService = inventorySnapshotService;
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
                throw new InvalidPnLCalculationException($"No InventorySnapshot for startDate {startDate}");

            if (endSnapshot == null)
                throw new InvalidPnLCalculationException($"No InventorySnapshot for endDate {endDate}");

            return PnLCalculator.GetPnL(startSnapshot, endSnapshot);
        }
    }
}
