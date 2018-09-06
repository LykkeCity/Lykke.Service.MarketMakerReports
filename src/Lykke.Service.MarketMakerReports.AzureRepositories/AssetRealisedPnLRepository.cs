using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
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

        public async Task<IReadOnlyCollection<AssetRealisedPnL>> GetAsync(string walletId, string assetId, DateTime date,
            int? limit)
        {
            string filterByPk = TableQuery.GenerateFilterCondition(nameof(AssetRealisedPnLEntity.PartitionKey),
                QueryComparisons.Equal, GetPartitionKey(walletId));

            string filterByPeriod = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition(nameof(AssetRealisedPnLEntity.RowKey), QueryComparisons.GreaterThan,
                    GetRowKey(date.Date.AddDays(1))),
                TableOperators.And,
                TableQuery.GenerateFilterCondition(nameof(AssetRealisedPnLEntity.RowKey), QueryComparisons.LessThan,
                    GetRowKey(date.Date.AddMilliseconds(-1))));

            string filterByAssetId = TableQuery.GenerateFilterCondition(nameof(AssetRealisedPnLEntity.AssetId),
                QueryComparisons.Equal, assetId);

            string filter = TableQuery.CombineFilters(filterByPk, TableOperators.And, filterByPeriod);

            filter = TableQuery.CombineFilters(filter, TableOperators.And, filterByAssetId);

            var query = new TableQuery<AssetRealisedPnLEntity>().Where(filter).Take(limit);

            IEnumerable<AssetRealisedPnLEntity> entities = await _storage.WhereAsync(query);

            return Mapper.Map<List<AssetRealisedPnL>>(entities);
        }

        public async Task<AssetRealisedPnL> GetLastAsync(string walletId, string assetId)
        {
            string filter = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition(nameof(AssetRealisedPnLEntity.PartitionKey),
                    QueryComparisons.Equal, GetPartitionKey(walletId)),
                TableOperators.And,
                TableQuery.GenerateFilterCondition(nameof(AssetRealisedPnLEntity.AssetId),
                    QueryComparisons.Equal, assetId));

            var query = new TableQuery<AssetRealisedPnLEntity>().Where(filter).Take(1);

            IEnumerable<AssetRealisedPnLEntity> entities = await _storage.WhereAsync(query);

            return Mapper.Map<AssetRealisedPnL>(entities.FirstOrDefault());
        }

        public async Task InsertAsync(AssetRealisedPnL assetRealisedPnL)
        {
            var entity = new AssetRealisedPnLEntity(GetPartitionKey(assetRealisedPnL.AssetId),
                GetRowKey(assetRealisedPnL.Time));

            Mapper.Map(assetRealisedPnL, entity);

            await _storage.InsertAsync(entity);
        }

        private string GetPartitionKey(string walletId)
            => walletId;

        private string GetRowKey(DateTime time)
            => (DateTime.MaxValue.Ticks - time.Ticks).ToString("D19");
    }
}
