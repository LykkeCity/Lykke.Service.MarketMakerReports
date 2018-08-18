using Lykke.AzureStorage.Tables;

namespace Lykke.Service.MarketMakerReports.AzureRepositories
{
    public class InventorySnapshotIndexEntity : AzureTableEntity
    {
        public InventorySnapshotIndexEntity()
        {
            
        }
        
        public InventorySnapshotIndexEntity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }
        
        public string ReferencePartitionKey { get; set; }
        
        public string ReferenceRowKey { get; set; }
    }
}
