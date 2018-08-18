using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots;
using Lykke.Service.MarketMakerReports.Core.Repositories;
using Lykke.Service.MarketMakerReports.Core.Services;
using Lykke.Service.RateCalculator.Client;
using Lykke.Service.RateCalculator.Client.AutorestClient.Models;

namespace Lykke.Service.MarketMakerReports.Services
{
    public class InventorySnapshotService : IInventorySnapshotService
    {
        private readonly IInventorySnapshotRepository _inventorySnapshotRepository;
        private readonly IRateCalculatorClient _rateCalculatorClient;
        private const string Usd = "USD";

        public InventorySnapshotService(IInventorySnapshotRepository inventorySnapshotRepository,
            IRateCalculatorClient rateCalculatorClient)
        {
            _inventorySnapshotRepository = inventorySnapshotRepository;
            _rateCalculatorClient = rateCalculatorClient;
        }
        
        public async Task HandleAsync(InventorySnapshot model)
        {
            var inventoryBalanceRecords = model.Assets
                .SelectMany(x => x.Inventories
                    .Select(i => new BalanceRecord(decimal.ToDouble(i.Volume), x.Asset)))
                .ToList();

            var balanceBalanceRecords = model.Assets
                .SelectMany(x => x.Balances
                    .Select(b => new BalanceRecord(decimal.ToDouble(b.Amount), x.Asset)))
                .ToList();

            var inUsd = (await GetInUsdAsync(inventoryBalanceRecords.Concat(balanceBalanceRecords))).ToList();

            var assetInventories = model.Assets.SelectMany(x => x.Inventories).Zip(inUsd.Take(inventoryBalanceRecords.Count), (inventory, usd) =>
            {
                inventory.VolumeInUsd = (decimal) usd.Balance;
                return inventory;
            }).ToList();

            var assetBalances = model.Assets.SelectMany(x => x.Balances).Zip(inUsd.Skip(inventoryBalanceRecords.Count), (balance, usd) =>
            {
                balance.AmountInUsd = (decimal) usd.Balance;
                return balance;
            }).ToList();

            await _inventorySnapshotRepository.InsertAsync(model);
        }

        private Task<IEnumerable<BalanceRecord>> GetInUsdAsync(IEnumerable<BalanceRecord> balanceRecords)
        {
            return _rateCalculatorClient.GetAmountInBaseAsync(balanceRecords, Usd);
        }

        public Task<IEnumerable<InventorySnapshot>> GetAsync(DateTime startDate, DateTime endDate)
        {
            return _inventorySnapshotRepository.GetAsync(startDate, endDate);
        }

        public async Task<(InventorySnapshot Start, InventorySnapshot End)> GetStartEndSnapshotsAsync(DateTime startDate, DateTime endDate)
        {
            var startSnapshot = await _inventorySnapshotRepository.GetFirstForDateAsync(startDate);
            var endSnapshot = await _inventorySnapshotRepository.GetLastForDateAsync(endDate);

            return (startSnapshot, endSnapshot);
        }
    }
}
