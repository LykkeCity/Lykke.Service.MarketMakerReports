using System.Collections.Generic;
using JetBrains.Annotations;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;

namespace Lykke.Service.MarketMakerReports.AzureRepositories
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateAlways)]
    public class WalletSettingsEntity : AzureTableEntity
    {
        public WalletSettingsEntity()
        {
        }

        public WalletSettingsEntity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        public bool HandleExternalTrades { get; set; }

        [JsonValueSerializer]
        public IReadOnlyCollection<string> Assets { get; set; }
    }
}
