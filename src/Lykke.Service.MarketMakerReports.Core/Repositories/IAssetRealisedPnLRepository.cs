using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain.PnL;

namespace Lykke.Service.MarketMakerReports.Core.Repositories
{
    public interface IAssetRealisedPnLRepository
    {
        Task<IReadOnlyCollection<AssetRealisedPnL>> GetAsync(string walletId, string assetId, DateTime date, int? limit);
        
        Task<AssetRealisedPnL> GetLastAsync(string walletId, string assetId);
        
        Task InsertAsync(AssetRealisedPnL assetRealisedPnL);
    }
}
