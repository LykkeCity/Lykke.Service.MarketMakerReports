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
        
        public DateTime Time { get; set; }
        
        public string Json { get; set; }
    }
}
