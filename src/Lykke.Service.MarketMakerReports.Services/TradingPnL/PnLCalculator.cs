using System.Collections.Generic;
using System.Linq;
using Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots;
using Lykke.Service.MarketMakerReports.Core.Domain.PnL;

namespace Lykke.Service.MarketMakerReports.Services.TradingPnL
{
    public static class PnLCalculator
    {
        public static PnLResult GetPnL(InventorySnapshot startInventorySnapshot, InventorySnapshot endInventorySnapshot)
        {
            string[] assets = GetAssets(startInventorySnapshot, endInventorySnapshot);

            string[] exchanges = GetExchanges(startInventorySnapshot, endInventorySnapshot);

            var exchangePnL = new List<ExchangePnL>();

            foreach (string exchange in exchanges)
            {
                var assetsPnL = new List<AssetPnL>();

                foreach (var assetId in assets)
                {
                    AssetPnL assetPnL = CalcPnLByAsset(assetId, exchange, startInventorySnapshot, endInventorySnapshot);

                    assetsPnL.Add(assetPnL);
                }

                exchangePnL.Add(new ExchangePnL
                {
                    Exchange = exchange,
                    Trading = assetsPnL.Sum(o => o.Trading),
                    Directional = assetsPnL.Sum(o => o.Directional),
                    AssetsPnLs = assetsPnL.Where(o => !o.IsEmpty()).ToList()
                });
            }

            return new PnLResult
            {
                StartDate = startInventorySnapshot.Timestamp,
                EndDate = endInventorySnapshot.Timestamp,
                ExchangePnLs = exchangePnL
            };
        }

        private static AssetPnL CalcPnLByAsset(string assetId, string exchange,
            InventorySnapshot startInventorySnapshot, InventorySnapshot endInventorySnapshot)
        {
            AssetBalanceInventory startAssetBalanceInventory = startInventorySnapshot.Assets
                .SingleOrDefault(a => a.AssetId == assetId);

            AssetBalanceInventory endAssetBalanceInventory = endInventorySnapshot.Assets
                .SingleOrDefault(a => a.AssetId == assetId);

            var startAssetBalance = startAssetBalanceInventory == null
                ? BalanceOnDate.Empty
                : new BalanceOnDate(startAssetBalanceInventory.GetBalanceByExchange(exchange));

            var endAssetBalance = endAssetBalanceInventory == null
                ? BalanceOnDate.Empty
                : new BalanceOnDate(endAssetBalanceInventory.GetBalanceByExchange(exchange));

            var startAssetInventory = startAssetBalanceInventory == null
                ? new AssetInventory {Exchange = exchange}
                : startAssetBalanceInventory.GetInventoryByExchange(exchange);

            var endAssetInventory = endAssetBalanceInventory == null
                ? new AssetInventory {Exchange = exchange}
                : endAssetBalanceInventory.GetInventoryByExchange(exchange);

            decimal tradingPnL = endAssetBalance.Price * (endAssetInventory.VolumeInUsd - startAssetInventory.VolumeInUsd);

            decimal directionalPnL = startAssetBalance.Balance * (endAssetBalance.Price - startAssetBalance.Price);

            var assetDisplayId = startAssetBalanceInventory?.AssetDisplayId ??
                                 endAssetBalanceInventory?.AssetDisplayId ?? assetId;

            return new AssetPnL
            {
                Asset = assetDisplayId,
                Exchange = exchange,
                Trading = tradingPnL,
                Directional = directionalPnL,
                StartBalance = startAssetBalance,
                EndBalance = endAssetBalance
            };
        }

        private static string[] GetAssets(InventorySnapshot startAssetInventory, InventorySnapshot endInventorySnapshot)
        {
            IEnumerable<string> startInventorySnapshotAssets = startAssetInventory.Assets
                .Select(o => o.AssetId);

            IEnumerable<string> endInventorySnapshotAssets = endInventorySnapshot.Assets
                .Select(o => o.AssetId);

            return startInventorySnapshotAssets
                .Union(endInventorySnapshotAssets)
                .ToArray();
        }

        private static string[] GetExchanges(InventorySnapshot startAssetInventory,
            InventorySnapshot endInventorySnapshot)
        {
            IEnumerable<string> startInventorySnapshotBalanceExchanges = startAssetInventory.Assets
                .SelectMany(x => x.Balances)
                .Select(x => x.Exchange);

            IEnumerable<string> startInventorySnapshotInventoryExchanges = startAssetInventory.Assets
                .SelectMany(x => x.Inventories)
                .Select(x => x.Exchange);

            IEnumerable<string> endInventorySnapshotBalanceExchanges = endInventorySnapshot.Assets
                .SelectMany(x => x.Balances)
                .Select(x => x.Exchange);

            IEnumerable<string> endInventorySnapshotInventoryExchanges = endInventorySnapshot.Assets
                .SelectMany(x => x.Inventories)
                .Select(x => x.Exchange);

            return startInventorySnapshotBalanceExchanges
                .Union(startInventorySnapshotInventoryExchanges)
                .Union(endInventorySnapshotBalanceExchanges)
                .Union(endInventorySnapshotInventoryExchanges)
                .ToArray();
        }
    }
}
