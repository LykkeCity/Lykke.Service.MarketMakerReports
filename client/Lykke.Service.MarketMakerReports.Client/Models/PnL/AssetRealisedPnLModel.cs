using System;
using JetBrains.Annotations;
using Lykke.Service.MarketMakerReports.Client.Models.Trades;

namespace Lykke.Service.MarketMakerReports.Client.Models.PnL
{
    /// <summary>
    /// Describes a cumulative profit and loss for trades by base asset at a certain point in time.
    /// </summary>
    [PublicAPI]
    public class AssetRealisedPnLModel
    {
        /// <summary>
        /// The asset id.
        /// </summary>
        public string AssetId { get; set; }
        
        /// <summary>
        /// The record time this time is equal to trade timestamp. 
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// The trade id.
        /// </summary>
        public string TradeId { get; set; }

        /// <summary>
        /// The name of exchange where trade was executed.
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// The asset pair of executed limit order. 
        /// </summary>
        public string AssetPair { get; set; }

        /// <summary>
        /// The trade price. 
        /// </summary>
        public decimal TradePrice { get; set; }

        /// <summary>
        /// The trade volume. 
        /// </summary>
        public decimal TradeVolume { get; set; }
        
        /// <summary>
        /// The trade type
        /// </summary>
        public TradeType TraderType { get; set; }
        
        /// <summary>
        /// The asset pair which used to convert quote asset to the calculated asset.
        /// </summary>
        public string CrossAssetPair { get; set; }

        /// <summary>
        /// The trade price in the calculated asset.
        /// </summary>
        public decimal Price { get; set; }
        
        /// <summary>
        /// The rate between quote assets. 
        /// </summary>
        public decimal CrossRate { get; set; }

        /// <summary>
        /// The trade volume in the calculated asset.
        /// </summary>
        public decimal OppositeVolume { get; set; }

        /// <summary>
        /// A ration between <see cref="CumulativeOppositeVolume"/> and <see cref="CumulativeVolume"/>.
        /// </summary>
        public decimal AvgPrice { get; set; }
        
        /// <summary>
        /// A cumulative not realised volume. 
        /// </summary>
        public decimal CumulativeVolume { get; set; }
        
        /// <summary>
        /// A cumulative not realised opposite volume.
        /// </summary>
        public decimal CumulativeOppositeVolume { get; set; }

        /// <summary>
        /// The realised PnL calculated by closing deal.
        /// </summary>
        public decimal RealisedPnL { get; set; }
        
        /// <summary>
        /// The <see cref="CumulativeVolume"/> realised by current rate.
        /// </summary>
        public decimal UnrealisedPnL { get; set; }
    }
}
