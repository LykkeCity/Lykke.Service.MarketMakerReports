using Autofac;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.MarketMakerReports.Managers;
using Lykke.Service.MarketMakerReports.Rabbit.Subscribers;
using Lykke.Service.MarketMakerReports.Settings;
using Lykke.Service.MarketMakerReports.Settings.ServiceSettings;
using Lykke.Service.NettingEngine.Client;
using Lykke.Service.RateCalculator.Client;
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
            builder.RegisterModule(new Services.AutofacModule());
            
            builder.RegisterModule(new AzureRepositories.AutofacModule(
                _appSettings.Nested(o => o.MarketMakerReportsService.Db.DataConnectionString)));
            
            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            RegisterRabbit(builder);
            
            builder.RegisterRateCalculatorClient(_appSettings.CurrentValue.RateCalculatorServiceClient.ServiceUrl);
            builder.RegisterNettingEngineClient(_appSettings.CurrentValue.NettingEngineServiceClient, null);
        }

        private void RegisterRabbit(ContainerBuilder builder)
        {
            RabbitSettings rabbitSettings = _appSettings.CurrentValue.MarketMakerReportsService.Rabbit;

            builder.RegisterType<AuditMessageSubscriber>()
                .AsSelf()
                .WithParameter(TypedParameter.From(rabbitSettings.AuditMessage))
                .SingleInstance();

            builder.RegisterType<InventorySnapshotSubscriber>()
                .AsSelf()
                .WithParameter(TypedParameter.From(rabbitSettings.InventorySnapshot))
                .SingleInstance();
        }
    }
}
