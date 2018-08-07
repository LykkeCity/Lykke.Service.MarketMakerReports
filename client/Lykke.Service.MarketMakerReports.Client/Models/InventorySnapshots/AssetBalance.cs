using Newtonsoft.Json;

namespace Lykke.Service.MarketMakerReports.Client.Models.InventorySnapshots
{
    public class AssetBalanceModel
    {
        [JsonIgnore]
        public string Asset { get; set; }
        
        public string Exchange { get; set; }
        
        public decimal Amount { get; set; }
        
        public decimal UsdEquivalent { get; set; }
    }
}
