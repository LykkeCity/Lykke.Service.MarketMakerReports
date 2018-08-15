using System;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain.PnL;

namespace Lykke.Service.MarketMakerReports.Core.Services
{
    public interface IPnLService
    {
        Task<PnLResult> CalculatePnLAsync(DateTime startDate, DateTime endDate);
    }
}
