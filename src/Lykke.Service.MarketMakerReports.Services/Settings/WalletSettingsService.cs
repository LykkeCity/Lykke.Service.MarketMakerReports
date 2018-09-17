using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.MarketMakerReports.Core.Domain.Settings;
using Lykke.Service.MarketMakerReports.Core.Extensions;
using Lykke.Service.MarketMakerReports.Core.Repositories;
using Lykke.Service.MarketMakerReports.Core.Services;

namespace Lykke.Service.MarketMakerReports.Services.Settings
{
    [UsedImplicitly]
    public class WalletSettingsService : IWalletSettingsService
    {
        private readonly IWalletSettingsRepository _walletSettingsRepository;
        private readonly ILog _log;

        private readonly WalletSettingsCache _walletSettingsCache = new WalletSettingsCache();

        public WalletSettingsService(
            IWalletSettingsRepository walletSettingsRepository,
            ILogFactory logFactory)
        {
            _walletSettingsRepository = walletSettingsRepository;
            _log = logFactory.CreateLog(this);
        }

        public async Task<IReadOnlyCollection<WalletSettings>> GetWalletsAsync()
        {
            IReadOnlyCollection<WalletSettings> walletsSettings = _walletSettingsCache.Get();

            if (walletsSettings.Count == 0)
            {
                walletsSettings = await _walletSettingsRepository.GetAllAsync();

                foreach (WalletSettings walletSettings in walletsSettings)
                    _walletSettingsCache.Set(walletSettings);
            }

            return walletsSettings;
        }

        public async Task<WalletSettings> GetWalletAsync(string walletId)
        {
            IReadOnlyCollection<WalletSettings> walletsSettings = await GetWalletsAsync();

            WalletSettings walletSettings = walletsSettings.SingleOrDefault(o => o.Id == walletId);

            if (walletSettings == null)
            {
                walletSettings = await _walletSettingsRepository.GetByIdAsync(walletId);

                if (walletSettings != null)
                    _walletSettingsCache.Set(walletSettings);
            }

            return walletSettings;
        }

        public async Task AddWalletAsync(WalletSettings walletSettings)
        {
            WalletSettings currentWalletSettings = await GetWalletAsync(walletSettings.Id);

            if (currentWalletSettings != null)
                throw new InvalidOperationException("Wallet already exists");

            await _walletSettingsRepository.InsertAsync(walletSettings);

            _log.InfoWithDetails("Wallet settings added", walletSettings);

            _walletSettingsCache.Set(walletSettings);
        }

        public async Task AddAssetToWalletAsync(string walletId, string assetId)
        {
            WalletSettings walletSettings = await GetWalletAsync(walletId);

            if (walletSettings == null)
                throw new InvalidOperationException("Wallet settings not found");

            if (walletSettings.Assets.Contains(assetId))
                throw new InvalidOperationException("Asset already exists");

            walletSettings.AddAsset(assetId);

            await _walletSettingsRepository.UpdateAsync(walletSettings);

            _log.InfoWithDetails($"Asset '{assetId}' added to the wallet settings", walletSettings);

            _walletSettingsCache.Set(walletSettings);
        }

        public async Task UpdateWalletAsync(WalletSettings walletSettings)
        {
            WalletSettings currentWalletSettings = await GetWalletAsync(walletSettings.Id);

            if (currentWalletSettings == null)
                throw new InvalidOperationException("Wallet settings not found");

            currentWalletSettings.Update(walletSettings);

            await _walletSettingsRepository.UpdateAsync(currentWalletSettings);

            _log.InfoWithDetails("Wallet settings updated", walletSettings);

            _walletSettingsCache.Set(currentWalletSettings);
        }

        public async Task DeleteWalletAsync(string walletId)
        {
            WalletSettings walletSettings = await GetWalletAsync(walletId);

            if (walletSettings == null)
                return;

            if (walletSettings.Enabled)
                throw new InvalidOperationException("Can not delete active wallet");

            if (walletSettings.Assets.Any())
                throw new InvalidOperationException("Can not delete wallet while at least one asset exist");

            await _walletSettingsRepository.DeleteAsync(walletId);

            _log.InfoWithDetails("Wallet settings deleted", walletSettings);

            _walletSettingsCache.Remove(walletId);
        }

        public async Task RemoveAssetFromWalletAsync(string walletId, string assetId)
        {
            WalletSettings walletSettings = await GetWalletAsync(walletId);

            if (walletSettings == null)
                throw new InvalidOperationException("Wallet settings not found");

            if (!walletSettings.Assets.Contains(assetId))
                return;

            if (walletSettings.Enabled)
                throw new InvalidOperationException("Can not delete asset while wallet is active");

            walletSettings.RemoveAsset(assetId);

            await _walletSettingsRepository.UpdateAsync(walletSettings);

            _log.InfoWithDetails($"Asset '{assetId}' removed from the wallet settings", walletSettings);

            _walletSettingsCache.Set(walletSettings);
        }
    }
}
