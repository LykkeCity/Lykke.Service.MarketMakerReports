using Newtonsoft.Json;

namespace Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots
{
    public class AssetInventory
    {
        [JsonIgnore]
        public string Asset { get; set; }
        
        public string Exchange { get; set; }
        
        public decimal Volume { get; set; }
        
        public decimal SellVolume { get; set; }
        
        public decimal BuyVolume { get; set; }
        
        public decimal UsdEquivalent { get; set; }
    }
}
