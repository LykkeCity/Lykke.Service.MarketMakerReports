using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Sdk;
using MoreLinq;

namespace Lykke.Service.MarketMakerReports.Managers
{
    [UsedImplicitly]
    public class StartupManager : IStartupManager
    {
        private readonly IStartable[] _startables;

        public StartupManager(IStartable[] startables)
        {
            _startables = startables;
        }
        
        public Task StartAsync()
        {
            _startables.ForEach(x => x.Start());
            
            return Task.CompletedTask;
        }
    }
}
