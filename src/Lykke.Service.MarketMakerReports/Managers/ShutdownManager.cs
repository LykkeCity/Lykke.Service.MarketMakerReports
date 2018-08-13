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
        private readonly LykkeTradeSubscriber _lykkeTradeSubscriber;
        private readonly ExternalTradeSubscriber _externalTradeSubscriber;

        public ShutdownManager(
            AuditMessageSubscriber auditMessageSubscriber,
            InventorySnapshotSubscriber inventorySnapshotSubscriber,
            LykkeTradeSubscriber lykkeTradeSubscriber,
            ExternalTradeSubscriber externalTradeSubscriber)
        {
            _auditMessageSubscriber = auditMessageSubscriber;
            _inventorySnapshotSubscriber = inventorySnapshotSubscriber;
            _lykkeTradeSubscriber = lykkeTradeSubscriber;
            _externalTradeSubscriber = externalTradeSubscriber;
        }
        
        public Task StopAsync()
        {
            _auditMessageSubscriber.Stop();
            _inventorySnapshotSubscriber.Stop();
            _lykkeTradeSubscriber.Stop();
            _externalTradeSubscriber.Stop();
            
            return Task.CompletedTask;
        }
    }
}
