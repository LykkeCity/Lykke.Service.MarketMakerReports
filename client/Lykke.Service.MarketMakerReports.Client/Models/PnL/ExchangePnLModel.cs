using System.Collections.Generic;
using JetBrains.Annotations;

namespace Lykke.Service.MarketMakerReports.Client.Models.PnL
{
    /// <summary>
    /// Represents an exchange profit and loss.
    /// </summary>
    [PublicAPI]
    public class ExchangePnLModel
    {
        /// <summary>
        /// Name of the Exchange
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// Trading part of the PnL (as result of trading) in USD
        /// </summary>
        public decimal Trading { get; set; }

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
