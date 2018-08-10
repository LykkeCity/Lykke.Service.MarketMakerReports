using System;
using System.Collections.Generic;

namespace Lykke.Service.MarketMakerReports.Client.Models.InventorySnapshots
{
    public class InventorySnapshotModel
    {
        public string Source { get; set; }
        
        public DateTime Timestamp { get; set; }
        
        public IReadOnlyList<AssetBalanceInventoryModel> Assets { get; set; }
    }
}
