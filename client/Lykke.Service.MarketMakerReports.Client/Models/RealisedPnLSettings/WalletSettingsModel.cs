using System.Collections.Generic;

namespace Lykke.Service.MarketMakerReports.Client.Models.RealisedPnLSettings
{
    /// <summary>
    /// Represents a wallet settings
    /// </summary>
    public class WalletSettingsModel
    {
        /// <summary>
        /// The wallet id
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// The wallet name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Indicated that the pnl calculation is enabled for this wallet
        /// </summary>
        public bool Enabled { get; set; }
        
        /// <summary>
        /// Indicated that the pnl calculation is allowed for external trades
        /// </summary>
        public bool HandleExternalTrades { get; set; }
        
        /// <summary>
        /// A collection of wallet assets
        /// </summary>
        public IReadOnlyCollection<string> Assets { get; set; }
    }
}
