using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.MarketMakerReports.Client.Models.Trades
{
    public class ExternalTradeModel
    {
        public string OrderId { get; set; }
        
        public string Exchange { get; set; }

        public string AssetPairId { get; set; }

        public TradeType Type { get; set; }

        [JsonConverter(typeof (StringEnumConverter))]
        public DateTime Time { get; set; }

        public decimal Price { get; set; }

        public decimal Volume { get; set; }
    }
}
