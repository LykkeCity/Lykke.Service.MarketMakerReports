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
    public class LykkeTradeRepository : ILykkeTradeRepository
    {
        private readonly INoSQLTableStorage<LykkeTradeEntity> _storage;

        public LykkeTradeRepository(INoSQLTableStorage<LykkeTradeEntity> storage)
        {
            _storage = storage;
        }

        public async Task<(IReadOnlyList<LykkeTrade> entities, string continuationToken)> GetAsync(DateTime startDate, 
            DateTime endDate, int? limit, string continuationToken)
        {
            var filter = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition(nameof(LykkeTradeEntity.PartitionKey), QueryComparisons.GreaterThan,
                    GetPartitionKey(startDate)),
                TableOperators.And,
                TableQuery.GenerateFilterCondition(nameof(LykkeTradeEntity.PartitionKey), QueryComparisons.LessThan,
                    GetPartitionKey(endDate)));

            var query = new TableQuery<LykkeTradeEntity>().Where(filter).Take(limit);

            (IEnumerable<LykkeTradeEntity> entities, string token) = 
                await _storage.GetDataWithContinuationTokenAsync(query, continuationToken);

            return (Mapper.Map<List<LykkeTrade>>(entities), token);
        }

        public async Task InsertAsync(LykkeTrade lykkeTrade)
        {
            var entity = new LykkeTradeEntity(GetPartitionKey(lykkeTrade.Time), GetRowKey(lykkeTrade.Id));

            Mapper.Map(lykkeTrade, entity);
            
            await _storage.InsertOrReplaceAsync(entity);
        }

        private string GetRowKey(string id)
        {
            return id;
        }

        private string GetPartitionKey(DateTime time)
        {
            return $"{time:yyyy-MM-ddTHH:mm}";
        }
    }
}
