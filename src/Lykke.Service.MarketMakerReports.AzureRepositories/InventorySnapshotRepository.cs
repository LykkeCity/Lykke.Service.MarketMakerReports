using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables.Templates.Index;
using Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots;
using Lykke.Service.MarketMakerReports.Core.Repositories;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace Lykke.Service.MarketMakerReports.AzureRepositories
{
    public class InventorySnapshotRepository : IInventorySnapshotRepository
    {
        private readonly INoSQLTableStorage<InventorySnapshotEntity> _storage;
        
        /// <summary>
        /// IndexStorage is used to quickly get the last Snapshot for any date.
        /// 
        /// IndexStorage keeps references for every day to the last InventorySnapshot
        /// for that particular day.
        /// 
        /// E.g. if the last snapshot in 2018-08-01 was received at 20:00:00, then
        /// index entry for that date must be as follows:
        /// 
        /// PartitionKey | RowKey | ReferencePartitionKey | ReferenceRowKey
        ///  2018-08-01  |  last  |   2018-08-01T20:00    | {id of the entry}
        /// </summary>
        private readonly INoSQLTableStorage<AzureIndex> _indexStorage;

        public InventorySnapshotRepository(INoSQLTableStorage<InventorySnapshotEntity> storage,
            INoSQLTableStorage<AzureIndex> indexStorage)
        {
            _storage = storage;
            _indexStorage = indexStorage;
        }

        public async Task<IEnumerable<InventorySnapshot>> GetAsync(DateTime startDate, DateTime endDate,
            Periodicity periodicity)
        {
            var entities = await GetEntitiesAsync(startDate, endDate);
            
            if (periodicity == Periodicity.OnePerDay)
            {
                entities = entities.GroupBy(x => x.Timestamp.ToString("yyyy-MM-dd"))
                    .Select(x => x.First());
            }
            else if (periodicity == Periodicity.OnePerHour)
            {
                entities = entities.GroupBy(x => x.Timestamp.ToString("yyyy-MM-ddTHH"))
                    .Select(x => x.First());
            }
            else if (periodicity == Periodicity.OnePerTwoDays)
            {
                var result = new List<InventorySnapshotEntity>();
                var entitiesList = entities.ToList();

                for (int i = 0; i < (endDate - startDate).Days; i += 2)
                {
                    var startOfTwoDaysInterval = startDate.AddDays(i);
                    var endOfTwoDaysInterval = startOfTwoDaysInterval.AddDays(1);

                    var entity = entitiesList
                        .FirstOrDefault(x => x.Timestamp >= startOfTwoDaysInterval && x.Timestamp <= endOfTwoDaysInterval);

                    if (entity != null)
                    {
                        result.Add(entity);    
                    }
                }
                
                entities = result;
            }

            return entities.Select(x => JsonConvert.DeserializeObject<InventorySnapshot>(x.Json));
        }
        
        public async Task InsertAsync(InventorySnapshot inventorySnapshot)
        {
            Guid id = Guid.NewGuid();
            
            var entity = new InventorySnapshotEntity(GetPartitionKey(inventorySnapshot.Timestamp), GetRowKey(id))
            {
                Time = inventorySnapshot.Timestamp,
                Json = JsonConvert.SerializeObject(inventorySnapshot)
            };

            await _storage.InsertAsync(entity);
            await UpdateIndexes(entity);
        }

        public async Task<InventorySnapshot> GetLastForDateAsync(DateTime date)
        {
            var index = await _indexStorage.GetDataAsync(GetIndexPartitionKey(date), IndexForLastRowKey);
            if (index == null)
            {
                // no index for date, try to build the index
                index = await BuildIndexForDate(date);
                
                if (index == null)
                {
                    // no data, can't return anything
                    return null;    
                }
            }

            var snapshot = await _storage.GetDataAsync(index);
            if (snapshot == null)
            {
                return null;
            }
            
            return JsonConvert.DeserializeObject<InventorySnapshot>(snapshot.Json);
        }

        public Task<InventorySnapshot> GetFirstForDateAsync(DateTime date)
        {
            return GetLastForDateAsync(date.AddDays(-1));
        }

        private async Task<IEnumerable<InventorySnapshotEntity>> GetEntitiesAsync(DateTime startDate, DateTime endDate)
        {
            var query = new TableQuery<InventorySnapshotEntity>().Where(FilterByDates(startDate, endDate));

            return await _storage.WhereAsync(query);
        }

        private static string FilterByDates(DateTime startDate, DateTime endDate)
        {
            return TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition(nameof(InventorySnapshotEntity.PartitionKey),
                    QueryComparisons.GreaterThanOrEqual,
                    GetPartitionKey(startDate)),
                TableOperators.And,
                TableQuery.GenerateFilterCondition(nameof(InventorySnapshotEntity.PartitionKey),
                    QueryComparisons.LessThanOrEqual,
                    GetPartitionKey(endDate)));
        }

        private async Task UpdateIndexes(InventorySnapshotEntity inventorySnapshotEntity)
        {
            var existingIndex = await _indexStorage.GetDataAsync(GetIndexPartitionKey(inventorySnapshotEntity.Time),
                IndexForLastRowKey);

            if (existingIndex == null || (await _storage.GetDataAsync(existingIndex)).Time < inventorySnapshotEntity.Time)
            {
                // Update index for current day, it now must point to this the last received snapshot
                await _indexStorage.InsertOrReplaceAsync(
                    AzureIndex.Create(GetIndexPartitionKey(inventorySnapshotEntity.Time), IndexForLastRowKey,
                        inventorySnapshotEntity));    
            }
        
            // If we have index entries for any future date, they are now are invalid, we can just remove them,
            // as they will be reconstructed if needed
            
            var filter = TableQuery.GenerateFilterCondition(nameof(AzureIndex.PartitionKey),
                QueryComparisons.GreaterThan,
                GetIndexPartitionKey(inventorySnapshotEntity.Time));
        
            var query = new TableQuery<AzureIndex>().Where(filter);

            var allNowInvalidIndexEntriesFromTheFuture = (await _indexStorage.WhereAsync(query)).ToList();

            foreach (var indexEntity in allNowInvalidIndexEntriesFromTheFuture)
            {
                await _indexStorage.DeleteAsync(indexEntity);
            }
        }

        private async Task<AzureIndex> BuildIndexForDate(DateTime date)
        {
            var snapshotsFromTheDay = (await GetEntitiesAsync(date.Date, date.Date.AddDays(1))).ToList();

            if (snapshotsFromTheDay.Any())
            {
                return await CreateIndexForLastEntry(date, snapshotsFromTheDay);
            }
            
            if (await CheckIfAnyBeforeDate(date))
            {
                // loop to the past day by day to find the previous snapshot and return it
                for (int i = 1; true; i++)
                {
                    var current = date.AddDays(-i);
                    var snapshotsForCurrentDay = (await GetEntitiesAsync(current.Date, current.Date.AddDays(1))).ToList();

                    if (snapshotsForCurrentDay.Any())
                    {
                        return await CreateIndexForLastEntry(date, snapshotsForCurrentDay);
                    }
                }
            }
            else if (await CheckIfAnyAfterDate(date))
            {
                // loot to the future day by day to find the FIRST snapshot and use it

                for (int i = 1; true; i++)
                {
                    var current = date.AddDays(i);
                    var snapshotsForCurrentDay = (await GetEntitiesAsync(current.Date, current.Date.AddDays(1))).ToList();

                    if (snapshotsForCurrentDay.Any())
                    {
                        return await CreateIndexForLastEntry(date, snapshotsForCurrentDay, getFirst: true);
                    }
                }
            }
            else
            {
                // no data, can't build index
                return null;    
            }
        }

        private async Task<AzureIndex> CreateIndexForLastEntry(DateTime date, IEnumerable<InventorySnapshotEntity> snapshotsFromTheDay, bool getFirst = false)
        {
            var lastFromTheDay = getFirst
                ? snapshotsFromTheDay.OrderBy(x => x.Time).First()
                : snapshotsFromTheDay.OrderBy(x => x.Time).Last();
            
            var index = AzureIndex.Create(GetIndexPartitionKey(date), IndexForLastRowKey, lastFromTheDay);

            await _indexStorage.InsertOrReplaceAsync(index);

            return index;
        }
        
        private async Task<bool> CheckIfAnyBeforeDate(DateTime date)
        {
            var filter = TableQuery.GenerateFilterCondition(nameof(InventorySnapshotEntity.PartitionKey),
                QueryComparisons.LessThan,
                GetPartitionKey(date));
            
            var query = new TableQuery<InventorySnapshotEntity>().Where(filter);

            var topRecord = await _storage.GetTopRecordAsync(query);

            return topRecord != null;
        }
        
        private async Task<bool> CheckIfAnyAfterDate(DateTime date)
        {
            var filter = TableQuery.GenerateFilterCondition(nameof(InventorySnapshotEntity.PartitionKey),
                QueryComparisons.GreaterThan,
                GetPartitionKey(date));
            
            var query = new TableQuery<InventorySnapshotEntity>().Where(filter);

            var topRecord = await _storage.GetTopRecordAsync(query);

            return topRecord != null;
        }

        private static string GetPartitionKey(DateTime date) => $"{date:yyyy-MM-ddTHH:mm}";

        private static string GetRowKey(Guid id) => $"{id:N}";

        private static string GetIndexPartitionKey(DateTime date) => $"{date:yyyy-MM-dd}";

        private static string IndexForLastRowKey = "last";
    }
}
