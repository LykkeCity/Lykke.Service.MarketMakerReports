using System;

namespace Lykke.Service.MarketMakerReports.Services.RealisedPnL
{
    public static class RealisedPnLCalculator
    {
        public static RealisedPnLResult Calculate(
            decimal tradeRate,
            decimal tradeVolume,
            bool inverted,
            int direction,
            decimal currentVolume,
            decimal currentOppositeVolume,
            decimal rate,
            decimal openRate,
            decimal crossRate)
        {
            var pnl = new RealisedPnLResult
            {
                CloseRate = inverted
                    ? 1 / tradeRate * crossRate
                    : tradeRate * crossRate,
                Volume = inverted
                    ? tradeRate * tradeVolume
                    : tradeVolume,
                OppositeVolume = inverted
                    ? tradeVolume * crossRate 
                    : tradeRate * tradeVolume * crossRate
            };

            if (currentVolume >= 0 && direction > 0 || currentVolume <= 0 && direction < 0)
            {
                pnl.CumulativeVolume = currentVolume + pnl.Volume * direction;
                pnl.CumulativeOppositeVolume = currentOppositeVolume + pnl.OppositeVolume * -1 * direction;
                pnl.AvgPrice = pnl.CumulativeVolume != 0
                    ? Math.Abs(pnl.CumulativeOppositeVolume / pnl.CumulativeVolume)
                    : 0;
            }
            else
            {
                if (pnl.Volume > Math.Abs(currentVolume))
                {
                    pnl.ClosedVolume = Math.Abs(currentVolume);
                    decimal openVolume = pnl.Volume - pnl.ClosedVolume;
                    pnl.CumulativeVolume = openVolume * direction;
                    pnl.CumulativeOppositeVolume = openVolume * pnl.CloseRate * -1 * direction;
                    pnl.AvgPrice = pnl.CloseRate;
                }
                else
                {
                    pnl.ClosedVolume = pnl.Volume;
                    pnl.CumulativeVolume = currentVolume + pnl.Volume * direction;
                    pnl.CumulativeOppositeVolume = currentOppositeVolume + pnl.Volume * openRate * -1 * direction;
                    pnl.AvgPrice = openRate;
                }

                pnl.RealisedPnL = (pnl.CloseRate - openRate) * pnl.ClosedVolume;
            }

            pnl.UnrealisedPnL = (rate - pnl.AvgPrice) * pnl.CumulativeVolume;

            return pnl;
        }
    }
}
