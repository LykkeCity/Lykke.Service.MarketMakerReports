using Lykke.Sdk.Health;

namespace Lykke.Service.MarketMakerReports.Controllers
{
    public class IsAliveController : Sdk.Controllers.IsAliveController
    {
        public IsAliveController(IHealthService healthService) : base(healthService)
        {
        }
    }
}
