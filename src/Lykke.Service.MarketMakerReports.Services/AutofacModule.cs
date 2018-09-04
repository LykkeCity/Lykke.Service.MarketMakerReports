using Autofac;
using JetBrains.Annotations;
using Lykke.Service.MarketMakerReports.Core.Services;

namespace Lykke.Service.MarketMakerReports.Services
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuditMessageService>()
                .As<IAuditMessageService>()
                .SingleInstance();

            builder.RegisterType<InventorySnapshotService>()
                .As<IInventorySnapshotService>()
                .SingleInstance();
            
            builder.RegisterType<LykkeTradeService>()
                .As<ILykkeTradeService>()
                .SingleInstance();
            
            builder.RegisterType<ExternalTradeService>()
                .As<IExternalTradeService>()
                .SingleInstance();

            builder.RegisterType<PnLService>()
                .As<IPnLService>()
                .SingleInstance();

            builder.RegisterType<HealthMonitorService>()
                .As<IHealthMonitorService>()
                .SingleInstance();
        }
    }
}
