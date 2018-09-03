using System;
using JetBrains.Annotations;
using Lykke.Service.MarketMakerReports.Core.Domain;
using Lykke.Service.MarketMakerReports.Core.Domain.PnL;
using Lykke.Service.MarketMakerReports.Core.Domain.Trades;
using Lykke.Service.MarketMakerReports.Core.Math;

namespace Lykke.Service.MarketMakerReports.Services.PnL
{
    [UsedImplicitly]
    public class AssetRealisedPnLCalculator : IAssetRealisedPnLCalculator
    {
        public AssetRealisedPnL Calculate(AssetRealisedPnL previousPnL, Trade trade, Quote quote, Quote crossQuote, string assetId)
        {
            int sign = trade.Type == TradeType.Sell ? -1 : 1;

            decimal oppositeVolume = trade.Price * trade.Volume * crossQuote.Mid;
            decimal cumulativeVolume;
            decimal cumulativeOppositeVolume;
            decimal realisedPnL;

            if (previousPnL.CumulativeVolume >= 0 && trade.Type == TradeType.Buy ||
                previousPnL.CumulativeVolume <= 0 && trade.Type == TradeType.Sell)
            {
                cumulativeVolume = previousPnL.CumulativeVolume + trade.Volume * sign;
                cumulativeOppositeVolume = previousPnL.CumulativeOppositeVolume + oppositeVolume * sign;
                realisedPnL = previousPnL.RealisedPnL;
            }
            else
            {
                decimal openPrice = previousPnL.AvgPrice;
                decimal closePrice = trade.Price / trade.Volume;
                decimal closeVolume;

                if (trade.Volume > Math.Abs(previousPnL.CumulativeVolume))
                {
                    closeVolume = Math.Abs(previousPnL.CumulativeVolume);
                    decimal remainingVolume = trade.Volume - closeVolume;
                    cumulativeVolume = remainingVolume * sign;
                    cumulativeOppositeVolume = remainingVolume * closePrice * sign;
                }
                else
                {
                    closeVolume = trade.Volume;
                    cumulativeVolume = previousPnL.CumulativeVolume + trade.Volume * sign;
                    cumulativeOppositeVolume = previousPnL.CumulativeOppositeVolume +
                                               trade.Volume * previousPnL.AvgPrice * sign;
                }

                realisedPnL = previousPnL.RealisedPnL + (closePrice - openPrice) * closeVolume;
            }

            decimal avgPrice = cumulativeVolume != 0 ? cumulativeOppositeVolume / cumulativeVolume : 0;

            decimal price = cumulativeVolume < 0
                ? quote.Ask
                : quote.Bid;

            decimal unrealisedPnL = (price - avgPrice) * cumulativeVolume;

            return new AssetRealisedPnL
            {
                AssetId = assetId,
                Time = trade.Time,
                TradeId = trade.Id,
                Exchange = trade.Exchange,
                AssetPair = trade.AssetPairId,
                TradePrice = trade.Price,
                TradeVolume = trade.Volume,
                TraderType = trade.Type,
                CrossAssetPair = crossQuote.AssetPair,
                Price = price,
                CrossRate = crossQuote.Mid,
                OppositeVolume = oppositeVolume,
                AvgPrice = avgPrice,
                CumulativeVolume = cumulativeVolume,
                CumulativeOppositeVolume = cumulativeOppositeVolume,
                RealisedPnL = realisedPnL,
                UnrealisedPnL = unrealisedPnL
            };
        }
    }
}
