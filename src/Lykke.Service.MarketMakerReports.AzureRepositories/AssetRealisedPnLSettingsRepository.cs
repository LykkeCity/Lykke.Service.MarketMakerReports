using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.MarketMakerReports.Core.Domain.Settings;
using Lykke.Service.MarketMakerReports.Core.Repositories;

namespace Lykke.Service.MarketMakerReports.AzureRepositories
{
    public class AssetRealisedPnLSettingsRepository : IAssetRealisedPnLSettingsRepository
    {
        private readonly INoSQLTableStorage<AssetRealisedPnLSettingsEntity> _storage;

        public AssetRealisedPnLSettingsRepository(INoSQLTableStorage<AssetRealisedPnLSettingsEntity> storage)
        {
            _storage = storage;
        }
        
        public async Task<AssetRealisedPnLSettings> GetAsync()
        {
            AssetRealisedPnLSettingsEntity entity = await _storage.GetDataAsync(GetPartitionKey(), GetRowKey());

            return Mapper.Map<AssetRealisedPnLSettings>(entity);
        }

        public async Task InsertOrReplaceAsync(AssetRealisedPnLSettings assetRealisedPnLSettings)
        {
            var entity = new AssetRealisedPnLSettingsEntity(GetPartitionKey(), GetRowKey());

            Mapper.Map(assetRealisedPnLSettings, entity);
            
            await _storage.InsertOrMergeAsync(entity);
        }

        private string GetPartitionKey()
            => "AssetRealisedPnLSettings";
        
        private string GetRowKey()
            => "AssetRealisedPnLSettings";
    }
}
