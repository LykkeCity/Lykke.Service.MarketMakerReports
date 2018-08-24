using System;

namespace Lykke.Service.MarketMakerReports.Core.Exceptions
{
    /// <summary>
    /// The exception is thrown if PnL calculation failed. The message should be shown in API response
    /// </summary>
    public class InvalidPnLCalculationException : Exception
    {
        public InvalidPnLCalculationException(string message) : base(message)
        {
        }
    }
}
