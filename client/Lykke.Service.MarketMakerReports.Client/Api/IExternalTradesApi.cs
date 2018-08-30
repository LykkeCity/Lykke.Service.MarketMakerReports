using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.MarketMakerReports.Client.Models.Trades;
using Refit;

namespace Lykke.Service.MarketMakerReports.Client.Api
{
    [PublicAPI]
    public interface IExternalTradesApi
    {
        [Get("/api/externaltrades")]
        Task<ContinuationTokenResult<ExternalTradeModel>> GetAsync(DateTime startDate, DateTime endDate,
            int? limit, [CanBeNull] string continuationToken);
    }
}
