using System;
using Lykke.Service.MarketMakerReports.Services.RealisedPnL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lykke.Service.MarketMakerReports.Services.Tests.RealisedPnL
{
    [TestClass]
    public class RealisedPnLCalculatorTests
    {
        [TestMethod]
        public void Open_Position()
        {
            // arrange

            decimal tradePrice = 6543.40m;
            decimal tradeVolume = 10;
            bool inverted = false;
            int direction = 1;
            decimal prevCumulativeVolume = 0;
            decimal prevCumulativeOppositeVolume = 0;
            decimal openRate = 0;
            decimal closeRate = 6543.40m;
            decimal crossRate = 1;
            
            var expectedResult = new RealisedPnLResult
            {
                AvgPrice = closeRate,
                Volume = tradeVolume,
                OppositeVolume = tradeVolume * tradePrice,
                CumulativeVolume = tradeVolume,
                CumulativeOppositeVolume = tradeVolume * tradePrice * -1 * direction,
                ClosedVolume = 0,
                RealisedPnL = 0,
                UnrealisedPnL = 0
            };
            
            // act

            RealisedPnLResult actualResult = RealisedPnLCalculator.Calculate(
                tradePrice,
                tradeVolume,
                inverted,
                direction,
                prevCumulativeVolume,
                prevCumulativeOppositeVolume,
                openRate,
                closeRate,
                crossRate);

            // assert
            
            Assert.IsTrue(AreEqual(expectedResult, actualResult));
        }

        [TestMethod]
        public void Increase_Opened_Position()
        {
            // arrange

            decimal tradePrice = 5607.87m;
            decimal tradeVolume = 1;
            bool inverted = false;
            int direction = 1;
            decimal prevCumulativeVolume = 10;
            decimal prevCumulativeOppositeVolume = -65434.00m;
            decimal openRate = 6543.40m;
            decimal closeRate = 6545.88m;
            decimal crossRate = 1.16m;

            var expectedResult = new RealisedPnLResult
            {
                Volume = tradeVolume,
                OppositeVolume = tradeVolume * tradePrice * crossRate,
                CumulativeVolume = prevCumulativeVolume + tradeVolume,
                CumulativeOppositeVolume =
                    prevCumulativeOppositeVolume + tradeVolume * tradePrice * crossRate * -1 * direction,
                ClosedVolume = 0,
                RealisedPnL = 0
            };

            expectedResult.AvgPrice =
                Math.Abs(expectedResult.CumulativeOppositeVolume / expectedResult.CumulativeVolume);
            
            expectedResult.UnrealisedPnL = (closeRate - expectedResult.AvgPrice) * expectedResult.CumulativeVolume;
            
            // act

            RealisedPnLResult actualResult = RealisedPnLCalculator.Calculate(
                tradePrice,
                tradeVolume,
                inverted,
                direction,
                prevCumulativeVolume,
                prevCumulativeOppositeVolume,
                openRate,
                closeRate,
                crossRate);

            // assert
            
            Assert.IsTrue(AreEqual(expectedResult, actualResult));
        }
        
        [TestMethod]
        public void Partially_Close_Opened_Position()
        {
            // arrange

            decimal tradePrice = 6544.95m;
            decimal tradeVolume = 6;
            bool inverted = false;
            int direction = -1;
            decimal prevCumulativeVolume = 11;
            decimal prevCumulativeOppositeVolume = -71939.13m;
            decimal openRate = 6539.92m;
            decimal closeRate = 6545.70m;
            decimal crossRate = 1m;
            
            var expectedResult = new RealisedPnLResult
            {
                AvgPrice = openRate,
                Volume = tradeVolume,
                OppositeVolume = tradeVolume * tradePrice * crossRate,
                CumulativeVolume = prevCumulativeVolume - tradeVolume,
                CumulativeOppositeVolume = prevCumulativeOppositeVolume + tradeVolume * openRate * -1 * direction,
                ClosedVolume = tradeVolume
            };

            expectedResult.RealisedPnL = (closeRate - openRate) * expectedResult.ClosedVolume;
            
            expectedResult.UnrealisedPnL = (closeRate - expectedResult.AvgPrice) * expectedResult.CumulativeVolume;
            
            // act

            RealisedPnLResult actualResult = RealisedPnLCalculator.Calculate(
                tradePrice,
                tradeVolume,
                inverted,
                direction,
                prevCumulativeVolume,
                prevCumulativeOppositeVolume,
                openRate,
                closeRate,
                crossRate);

            // assert
            
            Assert.IsTrue(AreEqual(expectedResult, actualResult));
        }
        
        [TestMethod]
        public void Open_Opposite_Position()
        {
            // arrange

            decimal tradePrice = 6543.65m;
            decimal tradeVolume = 8;
            bool inverted = false;
            int direction = -1;
            decimal prevCumulativeVolume = 5;
            decimal prevCumulativeOppositeVolume = -32699.61m;
            decimal openRate = 6539.92m;
            decimal closeRate = 6545.05m;
            decimal crossRate = 1m;
            
            var expectedResult = new RealisedPnLResult
            {
                AvgPrice = closeRate,
                Volume = tradeVolume,
                OppositeVolume = tradeVolume * tradePrice * crossRate,
                CumulativeVolume = prevCumulativeVolume - tradeVolume,
                CumulativeOppositeVolume = (prevCumulativeVolume - tradeVolume) * closeRate * -1 * direction,
                ClosedVolume = prevCumulativeVolume
            };

            expectedResult.RealisedPnL = (closeRate - openRate) * expectedResult.ClosedVolume;
            
            expectedResult.UnrealisedPnL = 0;
            
            // act

            RealisedPnLResult actualResult = RealisedPnLCalculator.Calculate(
                tradePrice,
                tradeVolume,
                inverted,
                direction,
                prevCumulativeVolume,
                prevCumulativeOppositeVolume,
                openRate,
                closeRate,
                crossRate);

            // assert
            
            Assert.IsTrue(AreEqual(expectedResult, actualResult));
        }
        
        private static bool AreEqual(RealisedPnLResult a, RealisedPnLResult b)
        {
            return a.AvgPrice == b.AvgPrice &&
                   a.Volume == b.Volume &&
                   a.OppositeVolume == b.OppositeVolume &&
                   a.CumulativeVolume == b.CumulativeVolume &&
                   a.CumulativeOppositeVolume == b.CumulativeOppositeVolume &&
                   a.ClosedVolume == b.ClosedVolume &&
                   a.RealisedPnL == b.RealisedPnL &&
                   a.UnrealisedPnL == b.UnrealisedPnL;
        }
    }
}
