using System;

namespace Lykke.Service.MarketMakerReports.Core.Domain.Trades
{
    public class ExternalTrade : Trade
    {
        public ExternalTrade()
        {
            // TODO: Add trade id to the rabbit mq contract 
            Id = Guid.NewGuid().ToString("D");
        }
        
        public string OrderId { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal OriginalVolume { get; set; }

        public decimal Commission { get; set; }

        public decimal ExchangeExecuteVolume { get; set; }
    }
}
