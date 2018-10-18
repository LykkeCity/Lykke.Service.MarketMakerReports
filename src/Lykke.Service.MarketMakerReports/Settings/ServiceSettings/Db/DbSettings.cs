using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.MarketMakerReports.Settings.ServiceSettings.Db
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class DbSettings
    {
        [AzureTableCheck]
        public string DataConnectionString { get; set; }

        [AzureTableCheck]
        public string LogsConnectionString { get; set; }
    }
}
