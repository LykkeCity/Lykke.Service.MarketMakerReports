namespace Lykke.Service.MarketMakerReports.Client.Models.InventorySnapshots
{
    public class AssetInventoryModel
    {
        public string Exchange { get; set; }
        
        public decimal Volume { get; set; }
        
        public decimal SellVolume { get; set; }
        
        public decimal BuyVolume { get; set; }
        
        public decimal VolumeInUsd { get; set; }
    }
}
