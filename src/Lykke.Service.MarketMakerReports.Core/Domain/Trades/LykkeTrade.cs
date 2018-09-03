namespace Lykke.Service.MarketMakerReports.Core.Domain.Trades
{
    public class LykkeTrade : Trade
    {
        public LykkeTrade()
        {
            Exchange = "lykke";
        }
        
        public decimal OppositeSideVolume { get; set; }

        public string LimitOrderId { get; set; }

        public string ExchangeOrderId { get; set; }

        public decimal RemainingVolume { get; set; }

        public TradeStatus Status { get; set; }

        public string OppositeClientId { get; set; }

        public string OppositeLimitOrderId { get; set; }
    }
}
