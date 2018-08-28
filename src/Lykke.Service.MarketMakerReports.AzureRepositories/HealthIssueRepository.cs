using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.MarketMakerReports.Core.Domain.Health;
using Lykke.Service.MarketMakerReports.Core.Repositories;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.MarketMakerReports.AzureRepositories
{
    public class HealthIssueRepository : IHealthIssueRepository
    {
        private readonly INoSQLTableStorage<HealthIssueEntity> _storage;

        public HealthIssueRepository(INoSQLTableStorage<HealthIssueEntity> storage)
        {
            _storage = storage;
        }

        public async Task<IReadOnlyList<HealthIssue>> GetAsync(DateTime startDate, DateTime endDate)
        {
            var filter = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition(nameof(HealthIssueEntity.PartitionKey), QueryComparisons.GreaterThanOrEqual,
                    GetPartitionKey(startDate)),
                TableOperators.And,
                TableQuery.GenerateFilterCondition(nameof(HealthIssueEntity.PartitionKey), QueryComparisons.LessThanOrEqual,
                    GetPartitionKey(endDate)));

            var query = new TableQuery<HealthIssueEntity>().Where(filter);
            
            IEnumerable<HealthIssueEntity> entities = await _storage.WhereAsync(query);

            return Mapper.Map<List<HealthIssue>>(entities);
        }
        
        public async Task InsertAsync(HealthIssue healthIssue)
        {
            var id = Guid.NewGuid();
            var entity = new HealthIssueEntity(GetPartitionKey(healthIssue.Time), GetRowKey(id));

            Mapper.Map(healthIssue, entity);
            
            await _storage.InsertOrReplaceAsync(entity);
        }

        private string GetRowKey(Guid id)
        {
            return id.ToString("N");
        }

        private string GetPartitionKey(DateTime time)
        {
            return $"{time:yyyy-MM-ddTHH:mm}";
        }
    }
}
