using System;
using System.Collections.Generic;

namespace Lykke.Service.MarketMakerReports.Client.Models.InventorySnapshots
{
    public class AssetInventoryDynamicsModel
    {
        public string Source { get; set; }
        
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public IReadOnlyList<AssetBalanceInventoryModel> Assets { get; set; }
    }
}
