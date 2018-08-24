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
        private readonly LykkeTradeSubscriber _lykkeTradeSubscriber;
        private readonly ExternalTradeSubscriber _externalTradeSubscriber;

        public StartupManager(
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
        
        public Task StartAsync()
        {
            _auditMessageSubscriber.Start();
            _inventorySnapshotSubscriber.Start();
            _lykkeTradeSubscriber.Start();
            _externalTradeSubscriber.Start();
            
            return Task.CompletedTask;
        }
    }
}
