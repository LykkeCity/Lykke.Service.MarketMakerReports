using System;
using System.Collections.Generic;

namespace Lykke.Service.MarketMakerReports.Client.Models.PnL
{
    /// <summary>
    /// PnL for a given period of time as a sum of PnLs by all the assets
    /// </summary>
    public class PnLResultModel
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
        /// The date of the first used for PnL calculation data
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// The date of the last used for PnL calculation data
        /// </summary>
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// PnLs by assets
        /// </summary>
        public IReadOnlyList<AssetPnLModel> AssetsPnLs { get; set; }
    }
}
