using System;
using System.Collections.Generic;
using System.Linq;
using Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots;

namespace Lykke.Service.MarketMakerReports.AzureRepositories
{
//    public static class InventorySnapshotEntityConverter
//    {
//        public static IReadOnlyList<InventorySnapshotEntity> ToEntities(string partitionKey, Guid id, InventorySnapshot inventorySnapshot)
//        {
//            var entities = new List<InventorySnapshotEntity>
//            {
//                FromInventorySnapshot(partitionKey, id, inventorySnapshot)
//            };
//
//            foreach (var assetBalanceInventory in inventorySnapshot.Assets)
//            {
//                entities.Add(FromAssetBalanceInventory(partitionKey, id, assetBalanceInventory));
//                entities.AddRange(assetBalanceInventory.Inventories.Select(assetInventory => FromInventory(partitionKey, id, assetInventory)));
//                entities.AddRange(assetBalanceInventory.Balances.Select(assetBalance => FromBalance(partitionKey, id, assetBalance)));
//            }
//
//            return entities;
//        }
//
//        private static InventorySnapshotEntity FromInventorySnapshot(string partitionKey, Guid id, 
//            InventorySnapshot inventorySnapshot)
//        {
//            return new InventorySnapshotEntity(partitionKey, GetRowKey(id, inventorySnapshot))
//            {
//                Timestamp = inventorySnapshot.Timestamp,
//                Source = inventorySnapshot.Source
//            };
//        }
//
//        private static InventorySnapshotEntity FromAssetBalanceInventory(string partitionKey, Guid id,
//            AssetBalanceInventory assetBalanceInventory)
//        {
//            return new InventorySnapshotEntity(partitionKey, GetRowKey(id, assetBalanceInventory))
//            {
//                Asset = assetBalanceInventory.Asset
//            };
//        }
//
//        private static InventorySnapshotEntity FromBalance(string partitionKey, Guid id, AssetBalance assetBalance)
//        {
//            return new InventorySnapshotEntity(partitionKey, GetRowKey(id, assetBalance))
//            {
//                Exchange = assetBalance.Exchange,
//                Amount = assetBalance.Amount,
//                UsdEquivalent = assetBalance.UsdEquivalent
//            };
//        }
//
//        private static InventorySnapshotEntity FromInventory(string partitionKey, Guid id,
//            AssetInventory assetInventory)
//        {
//            return new InventorySnapshotEntity(partitionKey, GetRowKey(id, assetInventory))
//            {
//                Exchange = assetInventory.Exchange,
//                Volume = assetInventory.Volume,
//                BuyVolume = assetInventory.BuyVolume,
//                SellVolume = assetInventory.SellVolume,
//                UsdEquivalent = assetInventory.UsdEquivalent
//            };
//        }
//        
//        private static string GetRowKey(Guid id, InventorySnapshot inventorySnapshot) => 
//            $"{id:N}_InventorySnapshot";
//
//        private static string GetRowKey(Guid id, AssetBalanceInventory assetBalanceInventory) => 
//            $"{id:N}_BalanceInventory_{assetBalanceInventory.Asset}";
//
//        private static string GetRowKey(Guid id, AssetBalance assetBalance) => 
//            $"{id:N}_Balance_{assetBalance.Exchange}";
//
//        private static string GetRowKey(Guid id, AssetInventory assetInventory) => 
//            $"{id:N}_Inventory_{assetInventory.Exchange}";
//
//        public static InventorySnapshot FromEntities(IEnumerable<InventorySnapshotEntity> inventorySnapshotEntities)
//        {
//            throw new NotImplementedException();
//        }
//    }
}
