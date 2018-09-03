using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain.PnL;
using Lykke.Service.MarketMakerReports.Core.Domain.Trades;

namespace Lykke.Service.MarketMakerReports.Core.Services
{
    public interface IAssetRealisedPnLService
    {
        Task<IReadOnlyList<AssetRealisedPnL>> GetAsync(string assetId, int? limit);
        
        Task CalculateAsync(Trade trade);
    }
}
