using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Sdk;
using MoreLinq;

namespace Lykke.Service.MarketMakerReports.Managers
{
    [UsedImplicitly]
    public class ShutdownManager : IShutdownManager
    {
        private readonly IStoppable[] _stoppables;
        
        public ShutdownManager(IStoppable[] stoppables)
        {
            _stoppables = stoppables;
        }
        
        public Task StopAsync()
        {
            _stoppables.ForEach(x => x.Stop());
            
            return Task.CompletedTask;
        }
    }
}
