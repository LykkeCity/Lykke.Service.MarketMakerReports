namespace Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots
{
    public class AssetPairInventory
    {
        public string AssetPair { get; set; }
        
        public decimal TotalSellBaseVolume { get; set; }
        
        public decimal TotalBuyBaseVolume { get; set; }
        
        public decimal TotalSellQuoteVolume { get; set; }
        
        public decimal TotalBuyQuoteVolume { get; set; }
        
        public int CountSellTrades { get; set; }
        
        public int CountBuyTrades { get; set; }
    }
}
