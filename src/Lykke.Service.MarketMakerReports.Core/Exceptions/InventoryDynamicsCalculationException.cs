using System;

namespace Lykke.Service.MarketMakerReports.Core.Exceptions
{
    /// <summary>
    /// The exception is thrown if inventory dynamics calculation failed. The message should be shown in API response
    /// </summary>
    public class InventoryDynamicsCalculationException : Exception
    {
        public InventoryDynamicsCalculationException(string message) : base(message)
        {
        }
    }
}
