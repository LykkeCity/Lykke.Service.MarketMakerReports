using System.Threading.Tasks;
using Lykke.Sdk;
using Lykke.Service.MarketMakerReports.Rabbit.Subscribers;

namespace Lykke.Service.MarketMakerReports.Managers
{
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
