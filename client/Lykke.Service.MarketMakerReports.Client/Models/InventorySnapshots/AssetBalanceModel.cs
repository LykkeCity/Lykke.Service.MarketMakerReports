namespace Lykke.Service.MarketMakerReports.Client.Models.InventorySnapshots
{
    public class AssetBalanceModel
    {
        public string Exchange { get; set; }
        
        public decimal Amount { get; set; }
        
        public decimal AmountInUsd { get; set; }
        
        public decimal Credit { get; set; }

        public decimal CreditInUsd { get; set; }
    }
}
