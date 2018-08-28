using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain.Health;

namespace Lykke.Service.MarketMakerReports.Core.Services
{
    public interface IHealthMonitorService
    {
        Task HandleAsync(HealthIssue healthIssue);
        
        Task<IReadOnlyList<HealthIssue>> GetAsync(DateTime startDate, DateTime endDate);
    }
}
