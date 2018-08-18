namespace Lykke.Service.MarketMakerReports.Client.Models.PnL
{
    public class AssetPnLModel
    {
        public string Asset { get; set; }

        public decimal Adjusted { get; set; }
        
        public decimal Directional { get; set; }

        public decimal Total => Adjusted + Directional;
        
        public decimal StartBalance { get; set; }
        
        public decimal StartBalanceInUsd { get; set;  }
        
        public decimal StartPrice { get; set; }
        
        
        public decimal EndBalance { get; set; }
        
        public decimal EndBalanceInUsd { get; set; }
        
        public decimal EndPrice { get; set; }
        
        public decimal SumOfChangeDepositOperations { get; set; }
    }
}
