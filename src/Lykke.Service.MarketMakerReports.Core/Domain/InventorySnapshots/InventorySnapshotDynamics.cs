using System;
using System.Collections.Generic;

namespace Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots
{
    public class InventorySnapshotDynamics
    {
        public string Source { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public IReadOnlyList<AssetBalanceInventory> Assets { get; set; }
        
        public IReadOnlyList<AssetPairInventory> AssetPairInventories { get; set; }
    }
}
