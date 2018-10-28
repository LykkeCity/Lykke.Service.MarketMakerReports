namespace Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots
{
    public class AssetBalance
    {
        public string Exchange { get; set; }
        
        public decimal Amount { get; set; }
        
        public decimal AmountInUsd { get; set; }
        
        public decimal Credit { get; set; }

        public decimal CreditInUsd { get; set; }
    }
}
