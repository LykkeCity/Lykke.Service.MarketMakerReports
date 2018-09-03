using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.MarketMakerReports.Client.Models.PnL;
using Refit;

namespace Lykke.Service.MarketMakerReports.Client.Api
{
    [PublicAPI]
    public interface IPnLApi
    {
        [Get("/api/pnl")]
        Task<PnLResultModel> GetAsync(DateTime startDate, DateTime endDate);

        [Get("/api/pnl/currentday")]
        Task<PnLResultModel> GetForCurrentDayAsync();

        [Get("/api/pnl/currentmonth")]
        Task<PnLResultModel> GetForCurrentMonthAsync();
        
        [Get("/api/pnl/realised")]
        Task<IReadOnlyList<AssetRealisedPnLModel>> GetRealisedAsync(string assetId, int? limit);
    }
}
