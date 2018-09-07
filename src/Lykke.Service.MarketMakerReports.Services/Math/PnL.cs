namespace Lykke.Service.MarketMakerReports.Services.Math
{
    public static class PnL
    {
        public static RealisedPnLResult CalculateRealisedPnl(
            decimal volume,
            decimal oppositeVolume,
            decimal avgPrice,
            decimal cumulativeVolume,
            decimal cumulativeOppositeVolume,
            decimal openPrice,
            decimal closePrice,
            int direction)
        {
            var pnl = new RealisedPnLResult();

            if (cumulativeVolume >= 0 && direction > 0 || cumulativeVolume <= 0 && direction < 0)
            {
                pnl.CumulativeVolume = cumulativeVolume + volume * direction;
                pnl.CumulativeOppositeVolume = cumulativeOppositeVolume + oppositeVolume * direction;
            }
            else
            {
                if (volume > System.Math.Abs(cumulativeVolume))
                {
                    pnl.ClosedVolume = System.Math.Abs(cumulativeVolume);
                    decimal openVolume = volume - pnl.ClosedVolume;
                    pnl.CumulativeVolume = openVolume * direction;
                    pnl.CumulativeOppositeVolume = openVolume * closePrice * direction;
                }
                else
                {
                    pnl.ClosedVolume = volume;
                    pnl.CumulativeVolume = cumulativeVolume + volume * direction;
                    pnl.CumulativeOppositeVolume = cumulativeOppositeVolume + volume * avgPrice * direction;
                }

                pnl.RealisedPnL = (closePrice - openPrice) * pnl.ClosedVolume;
            }

            return pnl;
        }
    }
}
