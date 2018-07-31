using Lykke.HttpClientGenerator;

namespace Lykke.Service.MarketMakerReports.Client
{
    /// <summary>
    /// MarketMakerReports API aggregating interface.
    /// </summary>
    public class MarketMakerReportsClient : IMarketMakerReportsClient
    {
        // Note: Add similar Api properties for each new service controller

        /// <summary>Inerface to MarketMakerReports Api.</summary>
        public IMarketMakerReportsApi Api { get; private set; }

        /// <summary>C-tor</summary>
        public MarketMakerReportsClient(IHttpClientGenerator httpClientGenerator)
        {
            Api = httpClientGenerator.Generate<IMarketMakerReportsApi>();
        }
    }
}
