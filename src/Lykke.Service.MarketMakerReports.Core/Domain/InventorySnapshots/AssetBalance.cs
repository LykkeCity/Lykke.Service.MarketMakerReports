using Newtonsoft.Json;

namespace Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots
{
    public class AssetBalance
    {
        [JsonIgnore]
        public string Asset { get; set; }
        
        public string Exchange { get; set; }
        
        public decimal Amount { get; set; }
        
        public decimal UsdEquivalent { get; set; }
    }
}
