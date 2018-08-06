using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.MarketMakerReports.Core.Domain.AuditMessages
{
    public class AuditMessage
    {
        public Guid Id { get; set; }
        
        public string InstanceName { get; set; }
        
        public DateTime CreationDate { get; set; }

        [JsonConverter(typeof (StringEnumConverter))]
        public AuditEventType EventType { get; set; }

        [JsonConverter(typeof (StringEnumConverter))]
        public SettingsChangeType SettingsChangeType { get; set; }

        public string ClientId { get; set; }

        public IDictionary<string, string> CurrentValues { get; set; }

        public IDictionary<string, string> PreviousValues { get; set; }
    }
}
