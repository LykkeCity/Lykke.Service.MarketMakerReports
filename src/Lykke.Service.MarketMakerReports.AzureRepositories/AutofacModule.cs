using Autofac;
using AzureStorage.Tables;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.MarketMakerReports.Core.Repositories;
using Lykke.SettingsReader;

namespace Lykke.Service.MarketMakerReports.AzureRepositories
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AutofacModule : Module
    {
        private readonly IReloadingManager<string> _connectionString;

        public AutofacModule(IReloadingManager<string> connectionString)
        {
            _connectionString = connectionString;
        }
        
        protected override void Load(ContainerBuilder builder)
        {
            const string auditMessagesTableName = "AuditMessages";
            const string inventorySnapshotsTableName = "InventorySnapshot";
            
            builder.Register(container => new AuditMessageRepository(
                    AzureTableStorage<AuditMessageEntity>.Create(_connectionString,
                        auditMessagesTableName, container.Resolve<ILogFactory>())))
                .As<IAuditMessageRepository>()
                .SingleInstance();

            builder.Register(container => new InventorySnapshotRepository(
                    AzureTableStorage<InventorySnapshotEntity>.Create(_connectionString,
                        inventorySnapshotsTableName, container.Resolve<ILogFactory>())))
                .As<IInventorySnapshotRepository>()
                .SingleInstance();
        }
    }
}
