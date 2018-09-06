using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.MarketMakerReports.Core.Domain.Settings;
using Lykke.Service.MarketMakerReports.Core.Repositories;
using Lykke.Service.MarketMakerReports.Core.Services;

namespace Lykke.Service.MarketMakerReports.Services.Settings
{
    [UsedImplicitly]
    public class WalletSettingsService : IWalletSettingsService
    {
        private readonly IWalletSettingsRepository _walletSettingsRepository;
        private readonly RealisedPnLSettingsCache _cache = new RealisedPnLSettingsCache();
        
        public WalletSettingsService(IWalletSettingsRepository walletSettingsRepository)
        {
            _walletSettingsRepository = walletSettingsRepository;
        }

        public async Task<IReadOnlyCollection<WalletSettings>> GetWalletsAsync()
        {
            IReadOnlyCollection<WalletSettings> walletsSettings = _cache.GetWallets();

            if (walletsSettings.Count == 0)
            {
                walletsSettings = await _walletSettingsRepository.GetAllAsync();

                foreach (WalletSettings walletSettings in walletsSettings)
                    _cache.Set(walletSettings);
            }

            return walletsSettings;
        }

        public async Task<WalletSettings> GetWalletAsync(string walletId)
        {
            WalletSettings walletSettings = _cache.GetWallet(walletId);

            if (walletSettings == null)
            {
                walletSettings = await _walletSettingsRepository.GetByIdAsync(walletId);
                
                if(walletSettings != null)
                    _cache.Set(walletSettings);
            }

            return walletSettings;
        }

        public async Task AddWalletAsync(WalletSettings walletSettings)
        {
            await _walletSettingsRepository.SaveAsync(walletSettings);
            
            _cache.Set(walletSettings);
        }

        public async Task AddAssetToWalletAsync(string walletId, string assetId)
        {
            WalletSettings walletSettings = await GetWalletAsync(walletId);
            
            if(walletSettings == null)
                throw new InvalidOperationException("Wallet settings not found");
            
            if(walletSettings.Assets.Contains(assetId))
                throw new InvalidOperationException("Asset already exists");
            
            walletSettings.AddAsset(assetId);
            
            await _walletSettingsRepository.SaveAsync(walletSettings);
            
            _cache.Set(walletSettings);
        }

        public async Task UpdateWalletAsync(WalletSettings walletSettings)
        {
            WalletSettings currentWalletSettings = await GetWalletAsync(walletSettings.Id);
            
            if(walletSettings == null)
                throw new InvalidOperationException("Wallet settings not found");
            
            currentWalletSettings.Update(walletSettings);
            
            await _walletSettingsRepository.SaveAsync(walletSettings);
            
            _cache.Set(currentWalletSettings);
        }
        
        public async Task DeleteWalletAsync(string walletId)
        {
            WalletSettings walletSettings = await GetWalletAsync(walletId);

            if (walletSettings == null)
                return;
            
            if(walletSettings.Enabled)
                throw new InvalidOperationException("Can not delete active wallet");
            
            if(walletSettings.Assets.Any())
                throw new InvalidOperationException("Can not delete wallet while at least one asset exist");

            await _walletSettingsRepository.DeleteAsync(walletId);
            
            _cache.Remove(walletId);
        }

        public async Task RemoveAssetFromWalletAsync(string walletId, string assetId)
        {
            WalletSettings walletSettings = await GetWalletAsync(walletId);
            
            if(walletSettings == null)
                throw new InvalidOperationException("Wallet settings not found");
            
            if(!walletSettings.Assets.Contains(assetId))
                return;
            
            if(walletSettings.Enabled)
                throw new InvalidOperationException("Can not delete asset while wallet is active");
            
            walletSettings.RemoveAsset(assetId);
            
            _cache.Set(walletSettings);

            await _walletSettingsRepository.SaveAsync(walletSettings);
        }
    }
}
