using System.Threading.Tasks;
using Lykke.Sdk;
using Lykke.Service.MarketMakerReports.Rabbit.Subscribers;

namespace Lykke.Service.MarketMakerReports.Managers
{
    public class StartupManager : IStartupManager
    {
        private readonly AuditMessageSubscriber _auditMessageSubscriber;

        public StartupManager(AuditMessageSubscriber auditMessageSubscriber)
        {
            _auditMessageSubscriber = auditMessageSubscriber;
        }
        
        public Task StartAsync()
        {
            _auditMessageSubscriber.Start();
            
            return Task.CompletedTask;
        }
    }
}
