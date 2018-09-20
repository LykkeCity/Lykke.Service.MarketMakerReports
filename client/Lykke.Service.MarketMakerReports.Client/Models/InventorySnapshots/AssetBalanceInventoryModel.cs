using System.Collections.Generic;

namespace Lykke.Service.MarketMakerReports.Client.Models.InventorySnapshots
{
    public class AssetBalanceInventoryModel
    {
        public string AssetId { get; set; }

        public string AssetDisplayId { get; set; }

        public IReadOnlyList<AssetInventoryModel> Inventories { get; set; }
        
        public IReadOnlyList<AssetBalanceModel> Balances { get; set; }
    }
}
