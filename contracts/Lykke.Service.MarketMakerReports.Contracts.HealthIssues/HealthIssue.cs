using System;

namespace Lykke.Service.MarketMakerReports.Contracts.HealthIssues
{
    public class HealthIssue
    {
        public string Source { get; set; }

        public DateTime Time { get; set; }

        public string Type { get; set; }

        public string Message { get; set; }
    }
}
