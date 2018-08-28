using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain.Health;
using Lykke.Service.MarketMakerReports.Core.Repositories;
using Lykke.Service.MarketMakerReports.Core.Services;

namespace Lykke.Service.MarketMakerReports.Services
{
    public class HealthMonitorService : IHealthMonitorService
    {
        private readonly IHealthIssueRepository _healthIssueRepository;

        public HealthMonitorService(IHealthIssueRepository healthIssueRepository)
        {
            _healthIssueRepository = healthIssueRepository;
        }
        
        public Task HandleAsync(HealthIssue healthIssue)
        {
            return _healthIssueRepository.InsertAsync(healthIssue);
        }

        public Task<IReadOnlyList<HealthIssue>> GetAsync(DateTime startDate, DateTime endDate)
        {
            return _healthIssueRepository.GetAsync(startDate, endDate);
        }
    }
}
