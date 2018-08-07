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
            FillAssetsInChildNodes(model);

            var inventories = model.Assets.SelectMany(x => x.Inventories).ToList();
            var balances = model.Assets.SelectMany(x => x.Balances).ToList();
            
            IEnumerable<BalanceRecord> balanceRecords =
                    inventories.Select(x => new BalanceRecord(decimal.ToDouble(x.Volume), x.Asset))
                .Concat(
                    balances.Select(x => new BalanceRecord(decimal.ToDouble(x.Amount), x.Asset)));

            var inUsd = (await GetInUsdAsync(balanceRecords)).ToList();

            inventories = inventories.Zip(inUsd.Take(inventories.Count), (inventory, usd) =>
                {
                    inventory.UsdEquivalent = (decimal) usd.Balance;
                    return inventory;
                }).ToList();

            balances = balances.Zip(inUsd.Skip(inventories.Count), (balance, usd) =>
                {
                    balance.UsdEquivalent = (decimal) usd.Balance;
                    return balance;
                }).ToList();

            await _inventorySnapshotRepository.InsertAsync(model);
        }

        private static void FillAssetsInChildNodes(InventorySnapshot model)
        {
            foreach (var asset in model.Assets)
            {
                foreach (var inventory in asset.Inventories)
                {
                    inventory.Asset = asset.Asset;
                }

                foreach (var balance in asset.Balances)
                {
                    balance.Asset = asset.Asset;
                }
            }
        }


        private Task<IEnumerable<BalanceRecord>> GetInUsdAsync(IEnumerable<BalanceRecord> balanceRecords)
        {
            return _rateCalculatorClient.GetAmountInBaseAsync(balanceRecords, Usd);
        }

        public Task<IEnumerable<InventorySnapshot>> GetAsync(DateTime startDate, DateTime endDate)
        {
            return _inventorySnapshotRepository.GetAsync(startDate, endDate);
        }
    }
}
