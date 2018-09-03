using System;

namespace Lykke.Service.MarketMakerReports.Core.Domain
{
    public class Quote
    {
        public Quote(string assetPair, DateTime time, decimal ask, decimal bid)
        {
            AssetPair = assetPair;
            Time = time;
            Ask = ask;
            Bid = bid;
            Mid = (ask + bid) / 2m;
            Spread = Ask - Bid;
        }

        public string AssetPair { get; }

        public DateTime Time { get; }

        public decimal Ask { get; }

        public decimal Bid { get; }

        public decimal Mid { get; }

        public decimal Spread { get; }
    }
}
