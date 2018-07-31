using JetBrains.Annotations;

namespace Lykke.Service.MarketMakerReports.Client
{
    /// <summary>
    /// MarketMakerReports client interface.
    /// </summary>
    [PublicAPI]
    public interface IMarketMakerReportsClient
    {
        /// <summary>Application Api interface</summary>
        IMarketMakerReportsApi Api { get; }
    }
}
