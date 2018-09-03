using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain.Trades;
using Lykke.Service.MarketMakerReports.Core.Repositories;
using Lykke.Service.MarketMakerReports.Core.Services;

namespace Lykke.Service.MarketMakerReports.Services
{
    public class LykkeTradeService : ILykkeTradeService
    {
        private readonly ILykkeTradeRepository _lykkeTradeRepository;

        public LykkeTradeService(ILykkeTradeRepository lykkeTradeRepository)
        {
            _lykkeTradeRepository = lykkeTradeRepository;
        }
        
        public Task HandleAsync(LykkeTrade lykkeTrade)
        {
            return _lykkeTradeRepository.InsertAsync(lykkeTrade);
        }

        public Task<(IReadOnlyList<LykkeTrade> entities, string continuationToken)> GetAsync(
            DateTime startDate, DateTime endDate, int? limit, string continuationToken)
        {
            return _lykkeTradeRepository.GetAsync(startDate, endDate, limit, continuationToken);
        }
    }
}
