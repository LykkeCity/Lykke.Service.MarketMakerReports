using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.MarketMakerReports.Client.Models.AuditMessages
{
    public class AuditMessageModel
    {
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
