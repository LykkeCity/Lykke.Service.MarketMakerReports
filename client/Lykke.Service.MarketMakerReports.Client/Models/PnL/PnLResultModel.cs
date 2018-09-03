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
        /// The date of the first used for PnL calculation data
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// The date of the last used for PnL calculation data
        /// </summary>
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// PnL info by exchange
        /// </summary>
        public IEnumerable<ExchangePnLModel> ExchangePnLs { get; set; }
        
        /// <summary>
        /// Total PnL info for all the exchanges
        /// </summary>
        public ExchangePnLModel OnAllExchanges { get; set; }
    }
}
