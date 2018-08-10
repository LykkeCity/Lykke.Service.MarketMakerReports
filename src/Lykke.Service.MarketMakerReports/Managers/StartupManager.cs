using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.MarketMakerReports.Rabbit.Subscribers;

namespace Lykke.Service.MarketMakerReports.Managers
{
    [UsedImplicitly]
    public class StartupManager : IStartupManager
    {
        private readonly AuditMessageSubscriber _auditMessageSubscriber;
        private readonly InventorySnapshotSubscriber _inventorySnapshotSubscriber;

        public StartupManager(AuditMessageSubscriber auditMessageSubscriber,
            InventorySnapshotSubscriber inventorySnapshotSubscriber)
        {
            _auditMessageSubscriber = auditMessageSubscriber;
            _inventorySnapshotSubscriber = inventorySnapshotSubscriber;
        }
        
        public Task StartAsync()
        {
            _auditMessageSubscriber.Start();
            _inventorySnapshotSubscriber.Start();
            
            return Task.CompletedTask;
        }
    }
}
