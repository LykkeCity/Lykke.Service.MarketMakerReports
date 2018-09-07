using System;
using Lykke.Service.MarketMakerReports.Core.Domain.Trades;

namespace Lykke.Service.MarketMakerReports.Core.Domain.PnL
{
    /// <summary>
    /// Describes a cumulative profit and loss for trades by base asset at a certain point in time.
    /// </summary>
    public class AssetRealisedPnL
    {
        /// <summary>
        /// The wallet id.
        /// </summary>
        public string WalletId { get; set; }
        
        /// <summary>
        /// The asset id.
        /// </summary>
        public string AssetId { get; set; }
        
        /// <summary>
        /// The record time this time is equal to trade timestamp. 
        /// </summary>
        public DateTime Time { get; set; }
        
        /// <summary>
        /// The name of exchange where trade was executed.
        /// </summary>
        public string Exchange { get; set; }
        
        /// <summary>
        /// The trade id.
        /// </summary>
        public string TradeId { get; set; }

        /// <summary>
        /// The asset pair of executed limit order. 
        /// </summary>
        public string TradeAssetPair { get; set; }

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
        public TradeType TradeType { get; set; }
        
        /// <summary>
        /// The asset pair which used to convert quote asset to the calculated asset.
        /// </summary>
        public string CrossAssetPair { get; set; }
        
        /// <summary>
        /// The rate between quote assets. 
        /// </summary>
        public decimal CrossPrice { get; set; }

        /// <summary>
        /// The trade price in the calculated asset.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The trade volume in the calculated asset.
        /// </summary>
        public decimal Volume { get; set; }
        
        /// <summary>
        /// The trade opposite volume in the calculated asset.
        /// </summary>
        public decimal OppositeVolume { get; set; }

        /// <summary>
        /// Indicates that the calculated volume is inverted.
        /// </summary>
        public bool Inverted { get; set; }
        
        /// <summary>
        /// The previous value of the average price.
        /// </summary>
        public decimal PrevAvgPrice { get; set; }

        /// <summary>
        /// The previous value of the cumulative volume.
        /// </summary>
        public decimal PrevCumulativeVolume { get; set; }

        /// <summary>
        /// The previous value of the cumulative opposite volume.
        /// </summary>
        public decimal PrevCumulativeOppositeVolume { get; set; }
        
        /// <summary>
        /// The volume of the closed position.
        /// </summary>
        public decimal CloseVolume { get; set; }

        /// <summary>
        /// The realised PnL of the closed position.
        /// </summary>
        public decimal RealisedPnL { get; set; }
        
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
        /// The cumulative realised PnL calculated by closing deal.
        /// </summary>
        public decimal CumulativeRealisedPnL { get; set; }
        
        /// <summary>
        /// The <see cref="CumulativeVolume"/> realised by current rate.
        /// </summary>
        public decimal UnrealisedPnL { get; set; }
        
        /// <summary>
        /// The opposite client id.
        /// </summary>
        /// <remarks>
        /// Available only on internal exchange. The <see cref="Guid.Empty"/> for external exchange.
        /// </remarks>
        public string OppositeClientId { get; set; }

        /// <summary>
        /// The opposite limit order id. Available only on internal exchange.
        /// </summary>
        /// <remarks>
        /// Available only on internal exchange. The <see cref="Guid.Empty"/> for external exchange.
        /// </remarks>
        public string OppositeLimitOrderId { get; set; }
    }
}
