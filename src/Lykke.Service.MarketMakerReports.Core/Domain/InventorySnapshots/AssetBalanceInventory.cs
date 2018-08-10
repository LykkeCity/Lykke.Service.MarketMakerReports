using System.Collections.Generic;

namespace Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots
{
    public class AssetBalanceInventory
    {
        public string Asset { get; set; }
        
        public IReadOnlyList<AssetInventory> Inventories { get; set; }
        
        public IReadOnlyList<AssetBalance> Balances { get; set; }
    }
}
