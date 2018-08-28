using System;
using Lykke.AzureStorage.Tables;

namespace Lykke.Service.MarketMakerReports.AzureRepositories
{
    public class HealthIssueEntity : AzureTableEntity
    {
        public HealthIssueEntity()
        {
            
        }

        public HealthIssueEntity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }
        
        public string Source { get; set; }
        
        public DateTime Time { get; set; }
        
        public string Type { get; set; }
        
        public string Message { get; set; }
    }
}
