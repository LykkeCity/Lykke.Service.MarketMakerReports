using System.Collections.Generic;

namespace Lykke.Service.MarketMakerReports.Core.Domain.Settings
{
    /// <summary>
    /// Represent a settings to calculate realised pnl by trades. 
    /// </summary>
    public class AssetRealisedPnLSettings
    {
        /// <summary>
        /// The assets to calculate PnL.
        /// </summary>
        public string AssetId { get; set; }

        /// <summary>
        /// A collection of base assets to calculate PnL.
        /// </summary>
        public IReadOnlyList<string> Assets { get; set; }
    }
}
