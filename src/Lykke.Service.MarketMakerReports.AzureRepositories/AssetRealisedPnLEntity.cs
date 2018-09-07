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

        public string WalletId { get; set; }

        public string AssetId { get; set; }

        public DateTime Time { get; set; }

        public string Exchange { get; set; }
        
        public string TradeId { get; set; }

        public string TradeAssetPair { get; set; }

        public decimal TradePrice { get; set; }

        public decimal TradeVolume { get; set; }

        public TradeType TradeType { get; set; }

        public string CrossAssetPair { get; set; }

        public decimal CrossPrice { get; set; }
        
        public decimal Price { get; set; }

        public decimal Volume { get; set; }

        public decimal OppositeVolume { get; set; }
        
        public bool Inverted { get; set; }
        
        public decimal PrevAvgPrice { get; set; }
        
        public decimal PrevCumulativeVolume { get; set; }
        
        public decimal PrevCumulativeOppositeVolume { get; set; }
        
        public decimal CloseVolume { get; set; }
        
        public decimal RealisedPnL { get; set; }

        public decimal AvgPrice { get; set; }

        public decimal CumulativeVolume { get; set; }

        public decimal CumulativeOppositeVolume { get; set; }

        public decimal CumulativeRealisedPnL { get; set; }

        public decimal UnrealisedPnL { get; set; }
        
        public string LimitOrderId { get; set; }
        
        public string OppositeClientId { get; set; }
        
        public string OppositeLimitOrderId { get; set; }
    }
}
