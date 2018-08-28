using System;

namespace Lykke.Service.MarketMakerReports.Client.Models.Health
{
    public class HealthIssueModel
    {
        public string Source { get; set; }
        
        public DateTime Time { get; set; }
        
        public string Type { get; set; }
        
        public string Message { get; set; }
    }
}
