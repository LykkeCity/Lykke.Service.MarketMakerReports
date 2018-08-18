namespace Lykke.Service.MarketMakerReports.Core.Domain.PnL
{
    public class AssetPnL
    {
        public string Asset { get; set; }

        public decimal Adjusted { get; set; }
        
        public decimal Directional { get; set; }

        public decimal Total => Adjusted + Directional;
        
        
        public BalanceOnDate StartBalance { get; set; }
        
        public BalanceOnDate EndBalance { get; set; }
        
        public decimal SumOfChangeDepositOperations { get; set; }

        public bool IsEmpty() => Adjusted == 0 &&
                                 Directional == 0 &&
                                 StartBalance.IsEmpty() &&
                                 EndBalance.IsEmpty() &&
                                 SumOfChangeDepositOperations == 0;
    }
}
