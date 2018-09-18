using System;
using System.Collections.Generic;
using System.Linq;
using Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots;

namespace Lykke.Service.MarketMakerReports.Services
{
    public class InventoryDynamicsCalculator
    {
        public InventorySnapshotDynamics GetDynamics(InventorySnapshot startSnapshot, InventorySnapshot endSnapshot)
        {
            if (startSnapshot == null)
                throw new ArgumentNullException(nameof(startSnapshot));

            if (endSnapshot == null)
                throw new ArgumentNullException(nameof(endSnapshot));

            Dictionary<string, AssetPairInventory> assetPairInventories = GroupAndMerge(
                startSnapshot.AssetPairInventories,
                endSnapshot.AssetPairInventories,
                o => o.AssetPair,
                ComputeAssetPairDynamics);

            Dictionary<string, AssetBalanceInventory> assetInventories = GroupAndMerge(
                startSnapshot.Assets,
                endSnapshot.Assets,
                o => o.AssetId,
                ComputeAssetDynamics);

            return new InventorySnapshotDynamics
            {
                Source = endSnapshot.Source,
                StartDate = startSnapshot.Timestamp,
                EndDate = endSnapshot.Timestamp,
                Assets = assetInventories.Values.ToList(),
                AssetPairInventories = assetPairInventories.Values.ToList()
            };
        }

        private static AssetBalanceInventory ComputeAssetDynamics(AssetBalanceInventory startInventory, AssetBalanceInventory endInventory)
        {
            if (startInventory == null && endInventory == null)
                throw new InvalidOperationException("At least one inventory is required to compute dynamics");

            Dictionary<string, AssetInventory> assetInventories = GroupAndMerge(
                startInventory?.Inventories,
                endInventory?.Inventories,
                o => o.Exchange,
                ComputeInventoryDynamics);

            Dictionary<string, AssetBalance> assetBalances = GroupAndMerge(
                startInventory?.Balances,
                endInventory?.Balances,
                o => o.Exchange,
                ComputeBalanceDynamics);

            return new AssetBalanceInventory
            {
                AssetId =
                    startInventory?.AssetId ?? endInventory.AssetId,
                AssetDisplayId =
                    startInventory?.AssetDisplayId ?? endInventory.AssetDisplayId,
                Inventories =
                    assetInventories.Values.ToList(),
                Balances =
                    assetBalances.Values.ToList()
            };
        }

        private static AssetInventory ComputeInventoryDynamics(AssetInventory startInventory, AssetInventory endInventory)
        {
            if (startInventory == null && endInventory == null)
                throw new InvalidOperationException("At least one inventory is required");

            decimal usdRate = (endInventory?.Volume ?? 0) != 0 ? endInventory.VolumeInUsd / endInventory.Volume : 0;
            decimal volume = (endInventory?.Volume ?? 0) - (startInventory?.Volume ?? 0);
            decimal volumeInUsd = endInventory != null ? volume * usdRate : -startInventory.VolumeInUsd;

            return new AssetInventory
            {
                Exchange = startInventory?.Exchange ?? endInventory.Exchange,
                Volume = volume,
                VolumeInUsd = volumeInUsd,
                SellVolume = (endInventory?.SellVolume ?? 0) - (startInventory?.SellVolume ?? 0),
                BuyVolume = (endInventory?.BuyVolume ?? 0) - (startInventory?.BuyVolume ?? 0)
            };
        }

        private static AssetBalance ComputeBalanceDynamics(AssetBalance startBalance, AssetBalance endBalance)
        {
            if (startBalance == null && endBalance == null)
                throw new InvalidOperationException("At least one balance is required");

            return new AssetBalance
            {
                Exchange = startBalance?.Exchange ?? endBalance.Exchange,
                Amount = endBalance?.Amount ?? 0,
                AmountInUsd = endBalance?.AmountInUsd ?? 0
            };
        }

        private static AssetPairInventory ComputeAssetPairDynamics(AssetPairInventory startInventory, AssetPairInventory endInventory)
        {
            if (startInventory == null && endInventory == null)
                throw new InvalidOperationException("At least one inventory is required");

            return new AssetPairInventory
            {
                AssetPair = startInventory?.AssetPair ?? endInventory.AssetPair,
                TotalSellBaseVolume =
                    (endInventory?.TotalSellBaseVolume ?? 0) - (startInventory?.TotalSellBaseVolume ?? 0),
                TotalBuyBaseVolume =
                    (endInventory?.TotalBuyBaseVolume ?? 0) - (startInventory?.TotalBuyBaseVolume ?? 0),
                TotalSellQuoteVolume =
                    (endInventory?.TotalSellQuoteVolume ?? 0) - (startInventory?.TotalSellQuoteVolume ?? 0),
                TotalBuyQuoteVolume =
                    (endInventory?.TotalBuyQuoteVolume ?? 0) - (startInventory?.TotalBuyQuoteVolume ?? 0),
                CountSellTrades =
                    (endInventory?.CountSellTrades ?? 0) - (startInventory?.CountSellTrades ?? 0),
                CountBuyTrades =
                    (endInventory?.CountBuyTrades ?? 0) - (startInventory?.CountBuyTrades ?? 0)
            };
        }

        /// <summary>
        /// Concatenates two sources, groups items by key selector and merges each group with specified operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source1">First source</param>
        /// <param name="source2">Second source</param>
        /// <param name="keySelector">Group key selector</param>
        /// <param name="operation">Merge operation</param>
        /// <returns>Dictionary where key is a result of keySelector and value is result of merge operation</returns>
        private static Dictionary<string, T> GroupAndMerge<T>(
            IEnumerable<T> source1, IEnumerable<T> source2, Func<T, string> keySelector, Func<T, T, T> operation)
        {
            var sourceOrder1 = (source1 ?? Enumerable.Empty<T>()).Select(o => (Item: o, Order: 1));
            var sourceOrder2 = (source2 ?? Enumerable.Empty<T>()).Select(o => (Item: o, Order: 2));

            Dictionary<string, T> merged =
                sourceOrder1.Concat(sourceOrder2)
                    .GroupBy(o => keySelector(o.Item))
                    .ToDictionary(
                        o => o.Key,
                        o => operation(
                            o.Where(e => e.Order == 1).Select(e => e.Item).FirstOrDefault(),
                            o.Where(e => e.Order == 2).Select(e => e.Item).FirstOrDefault()));

            return merged;
        }
    }
}
