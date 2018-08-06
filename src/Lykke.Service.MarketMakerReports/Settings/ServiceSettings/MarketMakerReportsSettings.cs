using JetBrains.Annotations;
using Lykke.Service.MarketMakerReports.Settings.ServiceSettings.Db;

namespace Lykke.Service.MarketMakerReports.Settings.ServiceSettings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class MarketMakerReportsSettings
    {
        public DbSettings Db { get; set; }
        
        public RabbitSettings Rabbit { get; set; }
    }
}
