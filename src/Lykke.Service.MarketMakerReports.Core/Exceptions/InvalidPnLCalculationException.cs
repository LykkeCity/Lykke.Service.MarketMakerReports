using System;

namespace Lykke.Service.MarketMakerReports.Core.Exceptions
{
    public class InvalidPnLCalculationException : Exception
    {
        public InvalidPnLCalculationException(string message) : base(message)
        {
        }
    }
}
