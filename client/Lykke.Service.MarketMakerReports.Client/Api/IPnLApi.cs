using System;
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
        Task<PnLResultModel> Calc(DateTime startDate, DateTime endDate);

        [Get("/api/pnl/currentday")]
        Task<PnLResultModel> CalcForCurrentDay();

        [Get("/api/pnl/currentmonth")]
        Task<PnLResultModel> CalcForCurrentMonth();
    }
}
