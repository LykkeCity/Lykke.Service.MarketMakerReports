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
        }
    }
}
