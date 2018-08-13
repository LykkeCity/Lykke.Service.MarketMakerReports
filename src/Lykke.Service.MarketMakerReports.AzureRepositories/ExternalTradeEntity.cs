using System;
using Lykke.AzureStorage.Tables;
using Lykke.Service.MarketMakerReports.Core.Domain.Trades;

namespace Lykke.Service.MarketMakerReports.AzureRepositories
{
    public class ExternalTradeEntity : AzureTableEntity
    {
        public ExternalTradeEntity()
        {
        }

        public ExternalTradeEntity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public string OrderId { get; set; }
        
        public string Exchange { get; set; }

        public string AssetPairId { get; set; }

        public TradeType Type { get; set; }

        public DateTime Time { get; set; }

        public decimal Price { get; set; }

        public decimal Volume { get; set; }
    }
}
