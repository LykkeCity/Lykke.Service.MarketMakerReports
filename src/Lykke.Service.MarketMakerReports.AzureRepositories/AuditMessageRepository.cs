using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.MarketMakerReports.Core.Domain.AuditMessages;
using Lykke.Service.MarketMakerReports.Core.Repositories;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.MarketMakerReports.AzureRepositories
{
    public class AuditMessageRepository : IAuditMessageRepository
    {
        private readonly INoSQLTableStorage<AuditMessageEntity> _storage;

        public AuditMessageRepository(INoSQLTableStorage<AuditMessageEntity> storage)
        {
            _storage = storage;
        }

        public async Task<IReadOnlyList<AuditMessage>> GetAsync(DateTime? startDate, DateTime? endDate, string clientId = null)
        {
            string filter = null;
            
            if (startDate.HasValue && endDate.HasValue)
            {
                filter = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition(nameof(AuditMessageEntity.PartitionKey), QueryComparisons.GreaterThanOrEqual,
                        GetPartitionKey(startDate.Value)),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition(nameof(AuditMessageEntity.PartitionKey), QueryComparisons.LessThanOrEqual,
                        GetPartitionKey(endDate.Value)));
            }
            else
            {
                if (startDate.HasValue)
                {
                    filter = TableQuery.GenerateFilterCondition(nameof(AuditMessageEntity.PartitionKey),
                        QueryComparisons.GreaterThanOrEqual,
                        GetPartitionKey(startDate.Value));
                }
                else if (endDate.HasValue)
                {
                    filter = TableQuery.GenerateFilterCondition(nameof(AuditMessageEntity.PartitionKey),
                        QueryComparisons.LessThanOrEqual,
                        GetPartitionKey(endDate.Value));
                }
            }

            if (!string.IsNullOrEmpty(clientId))
            {
                if (filter == default)
                {
                    filter = TableQuery.GenerateFilterCondition(nameof(AuditMessageEntity.ClientId),
                        QueryComparisons.Equal,
                        clientId);
                }
                else
                {
                    filter = TableQuery.CombineFilters(filter,
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition(nameof(AuditMessageEntity.ClientId), QueryComparisons.Equal,
                            clientId));    
                }
            }

            var query = new TableQuery<AuditMessageEntity>().Where(filter);
            
            IEnumerable<AuditMessageEntity> entities = await _storage.WhereAsync(query);

            return Mapper.Map<List<AuditMessage>>(entities);
        }

        public async Task<IReadOnlyList<AuditMessage>> GetAllAsync()
        {
            var entities = await _storage.GetDataAsync();

            return Mapper.Map<List<AuditMessage>>(entities);
        }

        public async Task InsertAsync(AuditMessage auditMessage)
        {
            var entity = new AuditMessageEntity(GetPartitionKey(auditMessage.CreationDate), GetRowKey(auditMessage.Id));

            Mapper.Map(auditMessage, entity);
            
            await _storage.InsertAsync(entity);
        }

        private static string GetPartitionKey(DateTime date)
            => $"{date:yyyy-MM-dd}";

        private static string GetRowKey(Guid id)
            => id.ToString("N");
    }
}
