namespace Lykke.Service.MarketMakerReports.Services.RealisedPnL
{
    public struct RealisedPnLResult
    {
        public decimal AvgPrice { get; set; }

        public decimal Price { get; set; }
        
        public decimal Volume { get; set; }

        public decimal OppositeVolume { get; set; }
        
        public decimal CumulativeVolume { get; set; }

        public decimal CumulativeOppositeVolume { get; set; }

        public decimal ClosedVolume { get; set; }
        
        public decimal RealisedPnL { get; set; }
        
        public decimal UnrealisedPnL { get; set; }
    }
}
