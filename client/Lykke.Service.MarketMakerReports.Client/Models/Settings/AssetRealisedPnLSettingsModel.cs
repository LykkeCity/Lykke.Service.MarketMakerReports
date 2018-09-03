using System.Collections.Generic;
using JetBrains.Annotations;

namespace Lykke.Service.MarketMakerReports.Client.Models.Settings
{
    /// <summary>
    /// Represent a settings to calculate realised pnl by trades. 
    /// </summary>
    [PublicAPI]
    public class AssetRealisedPnLSettingsModel
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
