using System;
using Lykke.AzureStorage.Tables;

namespace Lykke.Service.MarketMakerReports.AzureRepositories
{
    public class InventorySnapshotEntity : AzureTableEntity
    {
        public InventorySnapshotEntity()
        {
        }
        
        public InventorySnapshotEntity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }
        
        public string Json { get; set; }
        
//        public string Source { get; set; }
//        
//        public DateTime Timestamp { get; set; }
//        
//        public string Asset { get; set; }
//        
//        public string Exchange { get; set; }
//        
//        public decimal UsdEquivalent { get; set; }
//        
//        public decimal Volume { get; set; }
//        
//        public decimal SellVolume { get; set; }
//        
//        public decimal BuyVolume { get; set; }
//        
//        public decimal Amount { get; set; }
    }
}
