using System;
using Lykke.Service.MarketMakerReports.Core.Domain.Trades;

namespace Lykke.Service.MarketMakerReports.Services.PnL
{
    public class TradeData
    {
        public string Id { get; set; }

        public string Exchange { get; set; }

        public string AssetPair { get; set; }
        
        public string BaseAsset{ get; set; }
        
        public string QuoteAsset { get; set; }

        public decimal Price { get; set; }

        public decimal Volume { get; set; }

        public TradeType Type { get; set; }

        public DateTime Time { get; set; }

        public string LimitOrderId { get; set; }
        
        public string OppositeClientId { get; set; }
        
        public string OppositeLimitOrderId { get; set; }
    }
}
