using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.MarketMakerReports.Core.Domain.Settings;
using Lykke.Service.MarketMakerReports.Core.Repositories;
using Lykke.Service.MarketMakerReports.Core.Services;

namespace Lykke.Service.MarketMakerReports.Services.Settings
{
    [UsedImplicitly]
    public class AssetRealisedPnLSettingsService : IAssetRealisedPnLSettingsService
    {
        private readonly IAssetRealisedPnLSettingsRepository _assetRealisedPnLSettingsRepository;

        private AssetRealisedPnLSettings _assetRealisedPnLSettings;
        
        public AssetRealisedPnLSettingsService(IAssetRealisedPnLSettingsRepository assetRealisedPnLSettingsRepository)
        {
            _assetRealisedPnLSettingsRepository = assetRealisedPnLSettingsRepository;
        }
        
        public async Task<AssetRealisedPnLSettings> GetAsync()
        {
            if (_assetRealisedPnLSettings == null)
            {
                _assetRealisedPnLSettings = await _assetRealisedPnLSettingsRepository.GetAsync();

                if (_assetRealisedPnLSettings == null)
                {
                    _assetRealisedPnLSettings = new AssetRealisedPnLSettings
                    {
                        AssetId = "USD",
                        Assets = new List<string>()
                    };
                }
            }

            return _assetRealisedPnLSettings;
        }

        public async Task SaveAsync(AssetRealisedPnLSettings assetRealisedPnLSettings)
        {
            await _assetRealisedPnLSettingsRepository.InsertOrReplaceAsync(assetRealisedPnLSettings);
            
            _assetRealisedPnLSettings = assetRealisedPnLSettings;
        }
    }
}
