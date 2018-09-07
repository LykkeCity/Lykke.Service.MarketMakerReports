using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain.PnL;
using Lykke.Service.MarketMakerReports.Core.Domain.Trades;

namespace Lykke.Service.MarketMakerReports.Core.Services
{
    public interface IAssetRealisedPnLService
    {
        Task<IReadOnlyCollection<AssetRealisedPnL>> GetLastAsync(string walletId);

        Task<IReadOnlyCollection<AssetRealisedPnL>> GetByAssetAsync(string walletId, string assetId, DateTime date,
            int? limit);
        
        Task CalculateAsync(LykkeTrade lykkeTrade);

        Task CalculateAsync(ExternalTrade externalTrade);

        Task InitializeAsync(string walletId, string assetId, double amount);
    }
}
