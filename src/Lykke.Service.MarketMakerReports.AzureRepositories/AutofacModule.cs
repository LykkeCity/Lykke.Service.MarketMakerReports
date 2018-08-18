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
            const string inventorySnapshotsTableName = "InventorySnapshots";
            const string inventorySnapshotsIndexTableName = "InventorySnapshotsIndex";
            const string lykkeTradesTableName = "LykkeTrades";
            const string externalTradesTableName = "ExternalTrade";
            
            builder.Register(container => new AuditMessageRepository(
                    AzureTableStorage<AuditMessageEntity>.Create(_connectionString,
                        auditMessagesTableName, container.Resolve<ILogFactory>())))
                .As<IAuditMessageRepository>()
                .SingleInstance();

            builder.Register(container => new InventorySnapshotRepository(
                    AzureTableStorage<InventorySnapshotEntity>.Create(_connectionString,
                        inventorySnapshotsTableName, container.Resolve<ILogFactory>()),
                    AzureTableStorage<InventorySnapshotIndexEntity>.Create(_connectionString,
                        inventorySnapshotsIndexTableName, container.Resolve<ILogFactory>())))
                .As<IInventorySnapshotRepository>()
                .SingleInstance();

            builder.Register(container => new LykkeTradeRepository(
                    AzureTableStorage<LykkeTradeEntity>.Create(_connectionString,
                        lykkeTradesTableName, container.Resolve<ILogFactory>())))
                .As<ILykkeTradeRepository>()
                .SingleInstance();

            builder.Register(container => new ExternalTradeRepository(
                    AzureTableStorage<ExternalTradeEntity>.Create(_connectionString,
                        externalTradesTableName, container.Resolve<ILogFactory>())))
                .As<IExternalTradeRepository>()
                .SingleInstance();
        }
    }
}
