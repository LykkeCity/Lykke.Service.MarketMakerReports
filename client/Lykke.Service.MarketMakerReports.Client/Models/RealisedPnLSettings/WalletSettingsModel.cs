namespace Lykke.Service.MarketMakerReports.Client.Models.RealisedPnLSettings
{
    public class WalletSettingsModel
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public bool Enabled { get; set; }
        
        public bool HandleExternalTrades { get; set; }
    }
}
