using System;
using JetBrains.Annotations;
using Lykke.AzureStorage.Tables;
using Lykke.Service.MarketMakerReports.Core.Domain.Trades;

namespace Lykke.Service.MarketMakerReports.AzureRepositories
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AssetRealisedPnLEntity : AzureTableEntity
    {
        public AssetRealisedPnLEntity()
        {
        }

        public AssetRealisedPnLEntity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public string AssetId { get; set; }

        public DateTime Time { get; set; }

        public string TradeId { get; set; }

        public string Exchange { get; set; }

        public string AssetPair { get; set; }

        public decimal TradePrice { get; set; }

        public decimal TradeVolume { get; set; }

        public TradeType TraderType { get; set; }

        public string CrossAssetPair { get; set; }

        public decimal Price { get; set; }

        public decimal CrossRate { get; set; }

        public decimal OppositeVolume { get; set; }

        public decimal AvgPrice { get; set; }

        public decimal CumulativeVolume { get; set; }

        public decimal CumulativeOppositeVolume { get; set; }

        public decimal RealisedPnL { get; set; }

        public decimal UnrealisedPnL { get; set; }
    }
}
