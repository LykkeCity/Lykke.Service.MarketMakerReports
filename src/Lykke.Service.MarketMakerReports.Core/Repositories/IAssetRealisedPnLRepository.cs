using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain.PnL;

namespace Lykke.Service.MarketMakerReports.Core.Repositories
{
    public interface IAssetRealisedPnLRepository
    {
        Task<IReadOnlyList<AssetRealisedPnL>> GetAsync(string assetId, int? limit);

        Task<AssetRealisedPnL> GetLastAsync(string assetId);
        
        Task InsertAsync(AssetRealisedPnL assetRealisedPnL);
    }
}
