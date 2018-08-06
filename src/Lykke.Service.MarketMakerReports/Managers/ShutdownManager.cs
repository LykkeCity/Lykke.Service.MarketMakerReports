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

        public ShutdownManager(AuditMessageSubscriber auditMessageSubscriber)
        {
            _auditMessageSubscriber = auditMessageSubscriber;
        }
        
        public Task StopAsync()
        {
            _auditMessageSubscriber.Stop();
            
            return Task.CompletedTask;
        }
    }
}
