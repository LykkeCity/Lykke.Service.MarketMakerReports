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

        public async Task<IReadOnlyList<LykkeTrade>> GetAsync(DateTime startDate, DateTime endDate)
        {
            var filter = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition(nameof(LykkeTradeEntity.PartitionKey), QueryComparisons.GreaterThan,
                    GetPartitionKey(startDate)),
                TableOperators.And,
                TableQuery.GenerateFilterCondition(nameof(LykkeTradeEntity.PartitionKey), QueryComparisons.LessThan,
                    GetPartitionKey(endDate)));

            var query = new TableQuery<LykkeTradeEntity>().Where(filter);
            
            IEnumerable<LykkeTradeEntity> entities = await _storage.WhereAsync(query);

            return Mapper.Map<List<LykkeTrade>>(entities);
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
