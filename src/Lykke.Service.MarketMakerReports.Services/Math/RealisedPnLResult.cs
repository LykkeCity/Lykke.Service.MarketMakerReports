namespace Lykke.Service.MarketMakerReports.Services.Math
{
    public struct RealisedPnLResult
    {
        public decimal ClosedVolume { get; set; }

        public decimal RealisedPnL { get; set; }

        public decimal CumulativeVolume { get; set; }

        public decimal CumulativeOppositeVolume { get; set; }
    }
}
