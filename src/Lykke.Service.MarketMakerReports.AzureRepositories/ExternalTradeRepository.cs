using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.MarketMakerReports.Core.Domain.Trades;
using Lykke.Service.MarketMakerReports.Core.Repositories;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.MarketMakerReports.AzureRepositories
{
    public class ExternalTradeRepository : IExternalTradeRepository
    {
        private readonly INoSQLTableStorage<ExternalTradeEntity> _storage;

        public ExternalTradeRepository(INoSQLTableStorage<ExternalTradeEntity> storage)
        {
            _storage = storage;
        }
        
        public async Task<(IReadOnlyList<ExternalTrade> entities, string continuationToken)> GetAsync(DateTime startDate, 
            DateTime endDate, int? limit, string continuationToken)
        {
            var filter = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition(nameof(ExternalTradeEntity.PartitionKey), QueryComparisons.GreaterThan,
                    GetPartitionKey(startDate)),
                TableOperators.And,
                TableQuery.GenerateFilterCondition(nameof(ExternalTradeEntity.PartitionKey), QueryComparisons.LessThan,
                    GetPartitionKey(endDate)));

            var query = new TableQuery<ExternalTradeEntity>().Where(filter).Take(limit);

            (IEnumerable<ExternalTradeEntity> entities, string token) = 
                await _storage.GetDataWithContinuationTokenAsync(query, continuationToken);

            return (Mapper.Map<List<ExternalTrade>>(entities), token);
        }
        
        public async Task InsertAsync(ExternalTrade externalTrade)
        {
            var entity = new ExternalTradeEntity(GetPartitionKey(externalTrade.Time), GetRowKey(externalTrade));

            Mapper.Map(externalTrade, entity);
            
            await _storage.InsertOrReplaceAsync(entity);
        }

        private string GetRowKey(ExternalTrade externalTrade)
        {
            return $"{externalTrade.OrderId}_{externalTrade.Exchange}";
        }

        private string GetPartitionKey(DateTime time)
        {
            return $"{time:yyyy-MM-ddTHH:mm}";
        }
    }
}
