using Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots;

namespace Lykke.Service.MarketMakerReports.Core.Domain.PnL
{
    public class BalanceOnDate
    {
        public BalanceOnDate(AssetBalance assetBalance)
        {
            Balance = assetBalance.Amount;
            BalanceInUsd = assetBalance.AmountInUsd;
            Price = Balance == 0 ? 0 : BalanceInUsd / Balance;
        }
        
        public decimal Balance { get; }
        
        public decimal BalanceInUsd { get; }
        
        public decimal Price { get; }

        public bool IsEmpty() => Balance == 0 && BalanceInUsd == 0;
    }
}
