using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain.Trades;

namespace Lykke.Service.MarketMakerReports.Core.Repositories
{
    public interface ILykkeTradeRepository
    {
        Task InsertAsync(LykkeTrade lykkeTrade);
        
        Task<(IReadOnlyList<LykkeTrade> entities, string continuationToken)> GetAsync(DateTime startDate, DateTime endDate, 
            int? limit, string continuationToken);
    }
}
