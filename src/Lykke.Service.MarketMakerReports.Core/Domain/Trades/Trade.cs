using System;

namespace Lykke.Service.MarketMakerReports.Core.Domain.Trades
{
    public abstract class Trade
    {
        public string Id { get; set; }
        
        public string Exchange { get; set; }
        
        public string AssetPairId { get; set; }
        
        public decimal Price { get; set; }

        public decimal Volume { get; set; }
        
        public TradeType Type { get; set; }

        public DateTime Time { get; set; }
    }
}
