using System;
using System.Collections.Generic;

namespace Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots
{
    public class InventorySnapshot
    {
        public string Source { get; set; }
        
        public DateTime Timestamp { get; set; }
        
        public IReadOnlyList<AssetBalanceInventory> Assets { get; set; }
    }
}
