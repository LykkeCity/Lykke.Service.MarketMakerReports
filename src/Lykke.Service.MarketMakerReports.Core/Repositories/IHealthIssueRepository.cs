using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain.Health;

namespace Lykke.Service.MarketMakerReports.Core.Repositories
{
    public interface IHealthIssueRepository
    {
        Task InsertAsync(HealthIssue healthIssue);
        
        Task<IReadOnlyList<HealthIssue>> GetAsync(DateTime startDate, DateTime endDate);
    }
}
