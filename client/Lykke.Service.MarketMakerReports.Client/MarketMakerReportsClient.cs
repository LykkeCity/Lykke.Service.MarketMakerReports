using Lykke.HttpClientGenerator;
using Lykke.Service.MarketMakerReports.Client.Api;

namespace Lykke.Service.MarketMakerReports.Client
{
    /// <summary>
    /// MarketMakerReports API aggregating interface.
    /// </summary>
    public class MarketMakerReportsClient : IMarketMakerReportsClient
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MarketMakerReportsClient"/> with <param name="httpClientGenerator"></param>.
        /// </summary>
        public MarketMakerReportsClient(IHttpClientGenerator httpClientGenerator)
        {
            AuditMessagesApi = httpClientGenerator.Generate<IAuditMessagesApi>();
            InventorySnapshotsApi = httpClientGenerator.Generate<IInventorySnapshotsApi>();
            LykkeTradesApi = httpClientGenerator.Generate<ILykkeTradesApi>();
            ExternalTradesApi = httpClientGenerator.Generate<IExternalTradesApi>();
            PnLApi = httpClientGenerator.Generate<IPnLApi>();
            RealisedPnLSettings = httpClientGenerator.Generate<IRealisedPnLSettingsApi>();
            HealthIssuesApi = httpClientGenerator.Generate<IHealthIssuesApi>();
        }
        
        /// <summary>
        /// Api for AuditMessages
        /// </summary>
        public IAuditMessagesApi AuditMessagesApi { get; private set; }

        public IInventorySnapshotsApi InventorySnapshotsApi { get; }
        
        public ILykkeTradesApi LykkeTradesApi { get; }
        
        public IExternalTradesApi ExternalTradesApi { get; }
        
        public IPnLApi PnLApi { get; }

        public IRealisedPnLSettingsApi RealisedPnLSettings { get; }
        
        public IHealthIssuesApi HealthIssuesApi { get; }
    }
}
