using System;
using System.Collections.Generic;
using System.Linq;

namespace Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots
{
    public class AssetBalanceInventory
    {
        public string AssetId { get; set; }
        
        public string AssetDisplayId { get; set; }
        
        public IReadOnlyList<AssetInventory> Inventories { get; set; }
        
        public IReadOnlyList<AssetBalance> Balances { get; set; }

        public AssetBalance GetBalanceForExchange(string exchange)
        {
            return Balances.SingleOrDefault(x => string.Equals(x.Exchange, exchange, StringComparison.InvariantCultureIgnoreCase)) 
                   ?? new AssetBalance
                        {
                            Exchange = exchange
                        };
        }

        public decimal TotalBalance => Balances.Sum(x => x.Amount);

        public decimal TotalBalanceInUsd => Balances.Sum(x => x.AmountInUsd);

        public decimal PriceByBalanceInUsd => TotalBalance == 0 ? 0 : TotalBalanceInUsd / TotalBalance;
    }
}
