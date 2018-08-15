namespace Lykke.Service.MarketMakerReports.Core.Domain.PnL
{
    public class PnLResult
    {
        public PnLResult(decimal adjusted, decimal directional)
        {
            Adjusted = adjusted;
            Directional = directional;
        }

        public decimal Adjusted { get; }
        
        public decimal Directional { get; }

        public decimal Total => Adjusted + Directional;
    }
}
