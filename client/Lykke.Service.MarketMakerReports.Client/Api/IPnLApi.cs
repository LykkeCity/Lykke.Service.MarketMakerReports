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

        /// <summary>
        /// Returns a collection of latest realised pnl by wallet
        /// </summary>
        /// <param name="walletId">The wallet id</param>
        /// <returns>A collection of latest realised pnl</returns>
        [Get("/api/pnl/realised/{walletId}/assets")]
        Task<IReadOnlyList<AssetRealisedPnLModel>> GetLastRealisedPnlAsync(string walletId);

        /// <summary>
        /// Returns a collection of realised pnl by asset in wallet.
        /// </summary>
        /// <param name="walletId">The wallet id</param>
        /// <param name="assetId">The asset id</param>
        /// <param name="date">The date of realised pnl</param>
        /// <param name="limit">Optional parameter to limit numbers of records by asset</param>
        /// <returns>A collection of asset realised pnl</returns>
        [Get("/api/pnl/realised/{walletId}/assets/{assetId}")]
        Task<IReadOnlyList<AssetRealisedPnLModel>> GetRealisedByAssetAsync(string walletId, string assetId, DateTime date, int? limit);
    }
}
