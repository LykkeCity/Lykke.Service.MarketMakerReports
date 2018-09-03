using System.Collections.Generic;

namespace Lykke.Service.MarketMakerReports.Client.Models.PnL
{
    public class ExchangePnLModel
    {
        /// <summary>
        /// Adjusted part of the PnL (as result of trading) in USD
        /// </summary>
        public decimal Adjusted { get; set; }
        
        /// <summary>
        /// Directional part of the PnL (as result of price change), in USD
        /// </summary>
        public decimal Directional { get; set; }

        /// <summary>
        /// Total PnL as a sum of all the parts, in USD
        /// </summary>
        public decimal Total { get; set; }
        
        /// <summary>
        /// PnLs by assets
        /// </summary>
        public IReadOnlyList<AssetPnLModel> AssetsPnLs { get; set; }
    }
}
