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
        [Get("/api/realisedpnlsettigns/wallets")]
        Task<IReadOnlyCollection<WalletSettingsModel>> GetWalletsAsync();

        [Get("/api/realisedpnlsettigns/wallets/{walletId}")]
        Task<WalletSettingsModel> GetWalletAsync(string walletId);
        
        [Get("/api/realisedpnlsettigns/wallets/{walletId}/assets")]
        Task<IReadOnlyCollection<AssetSettingsModel>> GetAssetsAsync(string walletId);
        
        [Post("/api/realisedpnlsettigns/wallets")]
        Task AddWalletAsync(WalletSettingsModel walletSettingsModel);
        
        [Post("/api/realisedpnlsettigns/assets")]
        Task AddAssetAsync(AssetSettingsModel assetSettingsModel);
        
        [Put("/api/realisedpnlsettigns/wallets")]
        Task UpdateWalletAsync(WalletSettingsModel walletSettingsModel);
        
        [Delete("/api/realisedpnlsettigns/wallets/{walletId}")]
        Task DeleteWalletAsync(string walletId);
        
        [Delete("/api/realisedpnlsettigns/wallets/{walletId}/assets/{assetId}")]
        Task DeleteAssetAsync(string walletId, string assetId);
    }
}
