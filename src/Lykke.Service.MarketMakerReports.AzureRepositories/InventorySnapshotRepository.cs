using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots;
using Lykke.Service.MarketMakerReports.Core.Repositories;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace Lykke.Service.MarketMakerReports.AzureRepositories
{
    public class InventorySnapshotRepository : IInventorySnapshotRepository
    {
        private readonly INoSQLTableStorage<InventorySnapshotEntity> _storage;

        public InventorySnapshotRepository(INoSQLTableStorage<InventorySnapshotEntity> storage)
        {
            _storage = storage;
        }
        
        public async Task InsertAsync(InventorySnapshot inventorySnapshot)
        {
            Guid id = Guid.NewGuid();
            
            var entity = new InventorySnapshotEntity(GetPartitionKey(inventorySnapshot.Timestamp), GetRowKey(id))
            {
                Json = JsonConvert.SerializeObject(inventorySnapshot)
            };

            await _storage.InsertAsync(entity);
        }

        public async Task<IEnumerable<InventorySnapshot>> GetAsync(DateTime startDate, DateTime endDate)
        {
            var filter = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition(nameof(InventorySnapshotEntity.PartitionKey),
                    QueryComparisons.GreaterThanOrEqual,
                    GetPartitionKey(startDate)),
                TableOperators.And,
                TableQuery.GenerateFilterCondition(nameof(InventorySnapshotEntity.PartitionKey),
                    QueryComparisons.LessThanOrEqual,
                    GetPartitionKey(endDate)));
            
            var query = new TableQuery<InventorySnapshotEntity>().Where(filter);
            
            IEnumerable<InventorySnapshotEntity> entities = await _storage.WhereAsync(query);

            return entities.Select(x => JsonConvert.DeserializeObject<InventorySnapshot>(x.Json));
        }

        public async Task<InventorySnapshot> GetAsync(DateTime dateTime)
        {
            var startDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            var endDate = startDate.AddDays(1);

            var snapshots = await GetAsync(startDate, endDate);

            return snapshots.FirstOrDefault();
        }

        private static string GetPartitionKey(DateTime date) => $"{date:yyyy-MM-ddTHH:mm}";

        private static string GetRowKey(Guid id) => $"{id:N}";
    }
}
