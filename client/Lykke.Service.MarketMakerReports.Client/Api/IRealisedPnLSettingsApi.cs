using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.MarketMakerReports.Client.Models.RealisedPnLSettings;
using Refit;

namespace Lykke.Service.MarketMakerReports.Client.Api
{
    [PublicAPI]
    public interface IRealisedPnLSettingsApi
    {
        [Get("/api/realisedpnlsettings/wallets")]
        Task<IReadOnlyCollection<WalletSettingsModel>> GetWalletsAsync();

        [Get("/api/realisedpnlsettings/wallets/{walletId}")]
        Task<WalletSettingsModel> GetWalletAsync(string walletId);
        
        [Post("/api/realisedpnlsettings/wallets")]
        Task AddWalletAsync(WalletSettingsModel walletSettingsModel);
        
        [Post("/api/realisedpnlsettings/assets")]
        Task AddAssetToWalletAsync(AssetSettingsModel assetSettingsModel);
        
        [Put("/api/realisedpnlsettings/wallets")]
        Task UpdateWalletAsync(WalletSettingsModel walletSettingsModel);
        
        [Delete("/api/realisedpnlsettings/wallets/{walletId}")]
        Task DeleteWalletAsync(string walletId);
        
        [Delete("/api/realisedpnlsettings/wallets/{walletId}/assets/{assetId}")]
        Task RemoveAssetFromWalletAsync(string walletId, string assetId);
    }
}
