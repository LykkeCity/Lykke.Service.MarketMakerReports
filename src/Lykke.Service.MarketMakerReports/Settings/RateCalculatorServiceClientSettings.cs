using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.MarketMakerReports.Settings
{
    public class RateCalculatorServiceClientSettings
    {
        [HttpCheck("api/isalive")]
        public string ServiceUrl { get; set; }
    }
}
