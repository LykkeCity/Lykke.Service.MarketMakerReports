using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using Lykke.Service.MarketMakerReports.Settings.ServiceSettings;
using Lykke.Service.NettingEngine.Client;

namespace Lykke.Service.MarketMakerReports.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public MarketMakerReportsSettings MarketMakerReportsService { get; set; }
        
        public RateCalculatorServiceClientSettings RateCalculatorServiceClient { get; set; }
        
        public NettingEngineServiceClientSettings NettingEngineServiceClient { get; set; }
    }
}
