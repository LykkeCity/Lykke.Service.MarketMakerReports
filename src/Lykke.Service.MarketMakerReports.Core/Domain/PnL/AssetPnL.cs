namespace Lykke.Service.MarketMakerReports.Core.Domain.PnL
{
    public class AssetPnL
    {
        public string Asset { get; set; }

        public string Exchange { get; set; }

        public decimal Trading { get; set; }

        public decimal Directional { get; set; }

        public decimal Total => Trading + Directional;

        public BalanceOnDate StartBalance { get; set; }

        public BalanceOnDate EndBalance { get; set; }

        public bool IsEmpty() => Trading == 0 &&
                                 Directional == 0 &&
                                 StartBalance.IsEmpty() &&
                                 EndBalance.IsEmpty();
    }
}
