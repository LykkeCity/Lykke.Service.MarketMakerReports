using System.Collections.Generic;
using JetBrains.Annotations;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;

namespace Lykke.Service.MarketMakerReports.AzureRepositories
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AssetRealisedPnLSettingsEntity : AzureTableEntity
    {
        public AssetRealisedPnLSettingsEntity()
        {
        }

        public AssetRealisedPnLSettingsEntity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }
        
        public string AssetId { get; set; }

        [JsonValueSerializer]
        public IReadOnlyList<string> Assets { get; set; }
    }
}
