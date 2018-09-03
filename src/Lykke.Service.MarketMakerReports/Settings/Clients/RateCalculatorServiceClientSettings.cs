using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.MarketMakerReports.Settings.Clients
{
    [UsedImplicitly]
    public class RateCalculatorServiceClientSettings
    {
        [HttpCheck("api/isalive")]
        public string ServiceUrl { get; set; }
    }
}
