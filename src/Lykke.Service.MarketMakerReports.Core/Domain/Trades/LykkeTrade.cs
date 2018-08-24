using System;

namespace Lykke.Service.MarketMakerReports.Core.Domain.Trades
{
    public class LykkeTrade
    {
        public string AssetPairId { get; set; }

        public TradeType Type { get; set; }

        public DateTime Time { get; set; }

        public decimal Price { get; set; }

        public decimal Volume { get; set; }

        public decimal OppositeSideVolume { get; set; }

        public string Id { get; set; }

        public string LimitOrderId { get; set; }

        public string ExchangeOrderId { get; set; }

        public decimal RemainingVolume { get; set; }

        public TradeStatus Status { get; set; }

        public string OppositeClientId { get; set; }

        public string OppositeLimitOrderId { get; set; }
    }
}
