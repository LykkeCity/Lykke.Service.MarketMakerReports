using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain.Trades;
using Lykke.Service.MarketMakerReports.Core.Repositories;
using Lykke.Service.MarketMakerReports.Core.Services;

namespace Lykke.Service.MarketMakerReports.Services
{
    public class ExternalTradeService : IExternalTradeService
    {
        private readonly IExternalTradeRepository _externalTradeRepository;

        public ExternalTradeService(IExternalTradeRepository externalTradeRepository)
        {
            _externalTradeRepository = externalTradeRepository;
        }
        
        public Task HandleAsync(ExternalTrade externalTrade)
        {
            return _externalTradeRepository.InsertAsync(externalTrade);
        }

        public Task<(IReadOnlyList<ExternalTrade> entities, string continuationToken)> GetAsync(
            DateTime startDate, DateTime endDate, int? limit, string continuationToken)
        {
            return _externalTradeRepository.GetAsync(startDate, endDate, limit, continuationToken);
        }
    }
}
