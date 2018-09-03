using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain.Trades;

namespace Lykke.Service.MarketMakerReports.Core.Repositories
{
    public interface IExternalTradeRepository
    {
        Task InsertAsync(ExternalTrade externalTrade);
        
        Task<(IReadOnlyList<ExternalTrade> entities, string continuationToken)> GetAsync(DateTime startDate, DateTime endDate, 
            int? limit, string continuationToken);
    }
}
