using Lykke.HttpClientGenerator;
using Lykke.Service.MarketMakerReports.Client.Api;

namespace Lykke.Service.MarketMakerReports.Client
{
    /// <summary>
    /// MarketMakerReports API aggregating interface.
    /// </summary>
    public class MarketMakerReportsClient : IMarketMakerReportsClient
    {
        // Note: Add similar Api properties for each new service controller

        /// <summary>Api for AuditMessages</summary>
        public IAuditMessagesApi AuditMessagesApi { get; private set; }

        public IInventorySnapshotsApi InventorySnapshotsApi { get; }
        
        public ILykkeTradesApi LykkeTradesApi { get; }
        
        public IExternalTradesApi ExternalTradesApi { get; }
        
        public IPnLApi PnLApi { get; }
        
        public IHealthIssuesApi HealthIssuesApi { get; }

        /// <summary>C-tor</summary>
        public MarketMakerReportsClient(IHttpClientGenerator httpClientGenerator)
        {
            AuditMessagesApi = httpClientGenerator.Generate<IAuditMessagesApi>();
            InventorySnapshotsApi = httpClientGenerator.Generate<IInventorySnapshotsApi>();
            LykkeTradesApi = httpClientGenerator.Generate<ILykkeTradesApi>();
            ExternalTradesApi = httpClientGenerator.Generate<IExternalTradesApi>();
            PnLApi = httpClientGenerator.Generate<IPnLApi>();
            HealthIssuesApi = httpClientGenerator.Generate<IHealthIssuesApi>();
        }
    }
}
