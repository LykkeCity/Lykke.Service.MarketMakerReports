using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.MarketMakerReports.Client.Models.Health;
using Refit;

namespace Lykke.Service.MarketMakerReports.Client.Api
{
    [PublicAPI]
    public interface IHealthIssuesApi
    {
        [Get("/api/healthissues")]
        Task<IReadOnlyList<HealthIssueModel>> GetAsync(DateTime startDate, DateTime endDate);
    }
}
