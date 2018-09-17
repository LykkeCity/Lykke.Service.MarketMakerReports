namespace Lykke.Service.MarketMakerReports.Client.Models.RealisedPnLSettings
{
    /// <summary>
    /// Represents an asset settings 
    /// </summary>
    public class AssetSettingsModel
    {
        /// <summary>
        /// The asset id
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// The wallet id
        /// </summary>
        public string WalletId { get; set; }
        
        /// <summary>
        /// The initial amount of this asset
        /// </summary>
        public double Amount { get; set; }
    }
}
