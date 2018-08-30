using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.MarketMakerReports.Client.Models.Trades;
using Refit;

namespace Lykke.Service.MarketMakerReports.Client.Api
{
    [PublicAPI]
    public interface ILykkeTradesApi
    {
        [Get("/api/lykketrades")]
        Task<ContinuationTokenResult<LykkeTradeModel>> GetAsync(DateTime startDate, DateTime endDate, 
            int? limit, [CanBeNull] string continuationToken);
    }
}
