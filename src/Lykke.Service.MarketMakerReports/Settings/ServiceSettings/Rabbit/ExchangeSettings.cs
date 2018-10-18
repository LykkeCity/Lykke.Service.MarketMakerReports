using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.MarketMakerReports.Settings.ServiceSettings.Rabbit
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class ExchangeSettings
    {
        public ExchangeSettings()
        {
            Enabled = true;
        }

        [AmqpCheck]
        public string ConnectionString { get; set; }

        public string Exchange { get; set; }

        public string Queue { get; set; }

        [Optional]
        public bool Enabled { get; set; }
    }
}
