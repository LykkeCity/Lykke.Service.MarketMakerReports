using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.MarketMakerReports.Client.Models.Trades
{
    public class LykkeTradeModel
    {
        public string AssetPairId { get; set; }

        [JsonConverter(typeof (StringEnumConverter))]
        public TradeType Type { get; set; }

        public DateTime Time { get; set; }

        public decimal Price { get; set; }

        public decimal Volume { get; set; }

        public decimal OppositeSideVolume { get; set; }

        public string Id { get; set; }

        public string LimitOrderId { get; set; }

        public string ExchangeOrderId { get; set; }

        public decimal RemainingVolume { get; set; }
        
        [JsonConverter(typeof (StringEnumConverter))]
        public TradeStatus Status { get; set; }

        public string OppositeClientId { get; set; }

        public string OppositeLimitOrderId { get; set; }
    }
}
