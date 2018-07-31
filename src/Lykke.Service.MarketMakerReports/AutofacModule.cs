using Autofac;
using JetBrains.Annotations;
using Lykke.Service.MarketMakerReports.Settings;
using Lykke.SettingsReader;

namespace Lykke.Service.MarketMakerReports
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AutofacModule : Module
    {
        private readonly IReloadingManager<AppSettings> _appSettings;

        public AutofacModule(IReloadingManager<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
        }
    }
}
