using System;
using System.Collections.Generic;
using Common;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.Serializers;
using Lykke.Service.MarketMakerReports.Core.Domain.AuditMessages;

namespace Lykke.Service.MarketMakerReports.AzureRepositories
{
    public class AuditMessageEntity : AzureTableEntity
    {
        public AuditMessageEntity()
        {
        }

        public AuditMessageEntity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }
        
        public Guid Id { get; set; }
        
        public string InstanceName { get; set; }
        
        public DateTime CreationDate { get; set; }

        public AuditEventType EventType { get; set; }

        public SettingsChangeType SettingsChangeType { get; set; }

        public string ClientId { get; set; }

        [ValueSerializer(typeof(DictionarySerializer))]
        public IDictionary<string, string> CurrentValues { get; set; }

        [ValueSerializer(typeof(DictionarySerializer))]
        public IDictionary<string, string> PreviousValues { get; set; }
    }

    public class DictionarySerializer : IStorageValueSerializer
    {
        public string Serialize(object value, Type type)
        {
            return value.ToJson();
        }

        public object Deserialize(string serialized, Type type)
        {
            return serialized.DeserializeJson<Dictionary<string, string>>();
        }
    }
}
