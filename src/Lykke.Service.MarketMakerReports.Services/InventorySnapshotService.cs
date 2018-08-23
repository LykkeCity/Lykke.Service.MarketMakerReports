using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Common.Log;
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
        private readonly ILog _log;
        private const string Usd = "USD";

        public InventorySnapshotService(
            ILogFactory logFactory,
            IInventorySnapshotRepository inventorySnapshotRepository,
            IRateCalculatorClient rateCalculatorClient)
        {
            _log = logFactory.CreateLog(this);
            _inventorySnapshotRepository = inventorySnapshotRepository;
            _rateCalculatorClient = rateCalculatorClient;
        }
        
        public async Task HandleAsync(InventorySnapshot model)
        {
            try
            {
                await FillUsdForInventories(model);
                await FillUsdForBalances(model);
            }
            catch (Exception e)
            {
                _log.Error(e, context: $"Snapshot: {model.ToJson()}");
            }

            await _inventorySnapshotRepository.InsertAsync(model);
        }

        private async Task FillUsdForInventories(InventorySnapshot inventorySnapshot)
        {
            var balanceRecords = inventorySnapshot.Assets
                .SelectMany(x => x.Inventories.Select(i => new BalanceRecord(decimal.ToDouble(i.Volume), x.AssetId)))
                .ToList();

            var balanceRecordsInUsd = await GetInUsdAsync(balanceRecords);

            var flatListOfInventories = inventorySnapshot.Assets.SelectMany(x => x.Inventories).ToList();
            
            for (int i = 0; i < balanceRecords.Count; i++)
            {
                flatListOfInventories[i].VolumeInUsd = (decimal) balanceRecordsInUsd[i].Balance;
            }
        }

        private async Task FillUsdForBalances(InventorySnapshot inventorySnapshot)
        {
            var balanceRecords = inventorySnapshot.Assets
                .SelectMany(x => x.Balances.Select(b => new BalanceRecord(decimal.ToDouble(b.Amount), x.AssetId)))
                .ToList();

            var balanceRecordsInUsd = await GetInUsdAsync(balanceRecords);

            var flatListOfBalances = inventorySnapshot.Assets.SelectMany(x => x.Balances).ToList();
            
            for (int i = 0; i < balanceRecords.Count; i++)
            {
                flatListOfBalances[i].AmountInUsd = (decimal) balanceRecordsInUsd[i].Balance;
            }
        }

        private async Task<IReadOnlyList<BalanceRecord>> GetInUsdAsync(ICollection<BalanceRecord> balanceRecords)
        {
            var balanceRecordsInUsd = (await _rateCalculatorClient.GetAmountInBaseAsync(balanceRecords, Usd)).ToList();
            
            if (balanceRecords.Count != balanceRecordsInUsd.Count)
            {
                throw new InvalidOperationException("Wrong number of USD balances is returned. " +
                                                    $"Balance records in request: {balanceRecords.Count}, " +
                                                    $"balance records in response: {balanceRecordsInUsd.Count}");
            }

            return balanceRecordsInUsd;
        }

        public Task<IEnumerable<InventorySnapshot>> GetAsync(DateTime startDate, DateTime endDate, Periodicity periodicity)
        {
            return _inventorySnapshotRepository.GetAsync(startDate, endDate, periodicity);
        }

        public Task<InventorySnapshot> GetLastAsync()
        {
            return _inventorySnapshotRepository.GetLastForDateAsync(DateTime.UtcNow);
        }

        public async Task<(InventorySnapshot Start, InventorySnapshot End)> GetStartEndSnapshotsAsync(DateTime startDate, DateTime endDate)
        {
            var startSnapshot = await _inventorySnapshotRepository.GetFirstForDateAsync(startDate);
            var endSnapshot = await _inventorySnapshotRepository.GetLastForDateAsync(endDate);

            return (startSnapshot, endSnapshot);
        }
    }
}
