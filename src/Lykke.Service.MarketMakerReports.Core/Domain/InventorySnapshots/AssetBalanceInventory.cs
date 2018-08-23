using System.Collections.Generic;

namespace Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots
{
    public class AssetBalanceInventory
    {
        public string AssetId { get; set; }
        
        public string AssetDisplayId { get; set; }
        
        public IReadOnlyList<AssetInventory> Inventories { get; set; }
        
        public IReadOnlyList<AssetBalance> Balances { get; set; }
    }
}
