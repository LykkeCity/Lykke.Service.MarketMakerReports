using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Common;
using Lykke.Service.MarketMakerReports.Core.Domain.PnL;
using Lykke.Service.MarketMakerReports.Core.Repositories;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.MarketMakerReports.AzureRepositories
{
    public class AssetRealisedPnLRepository : IAssetRealisedPnLRepository
    {
        private readonly INoSQLTableStorage<AssetRealisedPnLEntity> _storage;

        public AssetRealisedPnLRepository(INoSQLTableStorage<AssetRealisedPnLEntity> storage)
        {
            _storage = storage;
        }
        
        public async Task<IReadOnlyList<AssetRealisedPnL>> GetAsync(string assetId, int? limit)
        {
            string filter = TableQuery.GenerateFilterCondition(nameof(AssetRealisedPnLEntity.PartitionKey),
                QueryComparisons.Equal, GetPartitionKey(assetId));
            
            var query = new TableQuery<AssetRealisedPnLEntity>().Where(filter).Take(limit);

            IEnumerable<AssetRealisedPnLEntity> entities = await _storage.WhereAsync(query);

            return Mapper.Map<List<AssetRealisedPnL>>(entities);
        }

        public async Task<AssetRealisedPnL> GetLastAsync(string assetId)
        {
            IReadOnlyList<AssetRealisedPnL> result = await GetAsync(assetId, 1);

            return result.FirstOrDefault();
        }

        public async Task InsertAsync(AssetRealisedPnL assetRealisedPnL)
        {
            var entity = new AssetRealisedPnLEntity(GetPartitionKey(assetRealisedPnL.AssetId),
                GetRowKey(assetRealisedPnL.Time));

            Mapper.Map(assetRealisedPnL, entity);

            await _storage.InsertAsync(entity);
        }

        private string GetPartitionKey(string assetId)
            => assetId;

        private string GetRowKey(DateTime time)
            => IdGenerator.GenerateDateTimeIdNewFirst(time);
    }
}
