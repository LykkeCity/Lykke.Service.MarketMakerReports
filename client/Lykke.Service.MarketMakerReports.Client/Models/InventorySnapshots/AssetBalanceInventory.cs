using System.Collections.Generic;

namespace Lykke.Service.MarketMakerReports.Client.Models.InventorySnapshots
{
    public class AssetBalanceInventoryModel
    {
        public string Asset { get; set; }
        
        public IReadOnlyList<AssetInventoryModel> Inventories { get; set; }
        
        public IReadOnlyList<AssetBalanceModel> Balances { get; set; }
    }
}
