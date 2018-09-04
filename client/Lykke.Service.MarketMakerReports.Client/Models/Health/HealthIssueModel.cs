using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.MarketMakerReports.Client.Models.Health
{
    public class HealthIssueModel
    {
        public string Source { get; set; }
        
        public DateTime Time { get; set; }
        
        public string Type { get; set; }
        
        public string Message { get; set; }
        
        public string Details { get; set; }
        
        [JsonConverter(typeof (StringEnumConverter))]
        public Severity Severity { get; set; }
    }
}
