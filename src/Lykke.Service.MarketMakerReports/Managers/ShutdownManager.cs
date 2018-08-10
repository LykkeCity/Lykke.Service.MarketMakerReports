using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.MarketMakerReports.Rabbit.Subscribers;

namespace Lykke.Service.MarketMakerReports.Managers
{
    [UsedImplicitly]
    public class ShutdownManager : IShutdownManager
    {
        private readonly AuditMessageSubscriber _auditMessageSubscriber;
        private readonly InventorySnapshotSubscriber _inventorySnapshotSubscriber;

        public ShutdownManager(AuditMessageSubscriber auditMessageSubscriber,
            InventorySnapshotSubscriber inventorySnapshotSubscriber)
        {
            _auditMessageSubscriber = auditMessageSubscriber;
            _inventorySnapshotSubscriber = inventorySnapshotSubscriber;
        }
        
        public Task StopAsync()
        {
            _auditMessageSubscriber.Stop();
            _inventorySnapshotSubscriber.Stop();
            
            return Task.CompletedTask;
        }
    }
}
