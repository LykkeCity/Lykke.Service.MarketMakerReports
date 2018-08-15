using System.Collections.Generic;
using System.Linq;

namespace Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots
{
    public class AssetBalanceInventory
    {
        public string Asset { get; set; }
        
        public IReadOnlyList<AssetInventory> Inventories { get; set; }
        
        public IReadOnlyList<AssetBalance> Balances { get; set; }
        
        public decimal TotalBalance => Balances.Sum(x => x.Amount);

        public decimal TotalBalanceInUsd => Balances.Sum(x => x.AmountInUsd);

        public decimal PriceByBalanceInUsd => TotalBalance == 0 ? 0 : TotalBalanceInUsd / TotalBalance;
    }
}
