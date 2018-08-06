using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.MarketMakerReports.Settings.ServiceSettings
{
    public class ExchangeSettings
    {
        [AmqpCheck]
        public string ConnectionString { get; set; }
        
        public string Exchange { get; set; }
        
        public string Queue { get; set; }
    }
}