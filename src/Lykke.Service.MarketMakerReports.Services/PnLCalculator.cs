using System;
using System.Collections.Generic;
using System.Linq;
using Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots;
using Lykke.Service.MarketMakerReports.Core.Domain.PnL;
using Lykke.Service.NettingEngine.Client.Models.Inventories;

namespace Lykke.Service.MarketMakerReports.Services
{
    public class PnLCalculator
    {
        public PnLResult GetPnL(InventorySnapshot startSnapshot, 
            InventorySnapshot endSnapshot,
            IReadOnlyList<ChangeDepositOperationSumModel> depositChanges)
        {
            var exchanges = startSnapshot.Assets.SelectMany(x => x.Balances).Select(x => x.Exchange)
                .Union(endSnapshot.Assets.SelectMany(x => x.Balances).Select(x => x.Exchange));
            
            var pnLsByExchange = exchanges.Select(x => 
                KeyValuePair.Create(x, GetPnL(startSnapshot, endSnapshot, depositChanges, x)))
                .ToDictionary(x => x.Key, x => x.Value);
            
            return new PnLResult
                {
                    StartDate = startSnapshot.Timestamp,
                    EndDate = endSnapshot.Timestamp,
                    ExchangePnLs = pnLsByExchange.Select(x => new ExchangePnL
                    {
                        Exchange = x.Key,
                        Adjusted = x.Value.Sum(v => v.Adjusted),
                        Directional = x.Value.Sum(v => v.Directional),
                        AssetsPnLs = x.Value.Where(v => !v.IsEmpty()).ToList()
                    })
                };
        }

        private IReadOnlyList<AssetPnL> GetPnL(InventorySnapshot startSnapshot, InventorySnapshot endSnapshot,
            IReadOnlyList<ChangeDepositOperationSumModel> changeDepositOperation,
            string exchange)
        {
            var assets = startSnapshot.Assets.Select(x => x.AssetId)
                .Union(endSnapshot.Assets.Select(x => x.AssetId));
            
            return assets.Select(x => CalcPnLForAsset(x,
                startSnapshot.Assets.SingleOrDefault(a => a.AssetId == x),
                endSnapshot.Assets.SingleOrDefault(a => a.AssetId == x),
                changeDepositOperation, exchange))
                .ToList();
        }

        private AssetPnL CalcPnLForAsset(string asset,
            AssetBalanceInventory start, 
            AssetBalanceInventory end,
            IReadOnlyList<ChangeDepositOperationSumModel> changeDepositOperations,
            string exchange)
        {
            var startAssetBalance = start == null ? BalanceOnDate.Empty : new BalanceOnDate(start.GetBalanceByExchange(exchange));
            var endAssetBalance = end == null ? BalanceOnDate.Empty : new BalanceOnDate(end.GetBalanceByExchange(exchange));

            var depositChanges = changeDepositOperations
                .Where(x => string.Equals(x.AssetId, asset, StringComparison.InvariantCultureIgnoreCase)
                            && string.Equals(x.Exchange, exchange, StringComparison.InvariantCultureIgnoreCase))
                .Sum(x => x.SumOfAllOperations);
            
            var inventory = endAssetBalance.Balance - startAssetBalance.Balance;
            var adjustedPnL = endAssetBalance.Price * (inventory - depositChanges);
            var directionalPnL = startAssetBalance.Balance * (endAssetBalance.Price - startAssetBalance.Price);

            return new AssetPnL
                {
                    Asset = asset,
                    Exchange = exchange,
                    Adjusted = adjustedPnL,
                    Directional = directionalPnL,
                    StartBalance = startAssetBalance,
                    EndBalance = endAssetBalance,
                    SumOfChangeDepositOperations = depositChanges
                };
        }
    }
}
