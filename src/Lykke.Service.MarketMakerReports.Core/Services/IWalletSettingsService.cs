using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain.Settings;

namespace Lykke.Service.MarketMakerReports.Core.Services
{
    public interface IWalletSettingsService
    {
        Task<IReadOnlyCollection<WalletSettings>> GetWalletsAsync();
        
        Task<WalletSettings> GetWalletAsync(string walletId);
        
        Task AddWalletAsync(WalletSettings walletSettings);
        
        Task AddAssetToWalletAsync(string walletId, string assetId);

        Task UpdateWalletAsync(WalletSettings walletSettings);
        
        Task DeleteWalletAsync(string walletId);
        
        Task RemoveAssetFromWalletAsync(string walletId, string assetId);
    }
}
