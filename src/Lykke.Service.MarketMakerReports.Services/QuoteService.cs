using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Service.Assets.Client;
using Lykke.Service.MarketMakerReports.Core.Domain;
using Lykke.Service.MarketMakerReports.Core.Services;
using Lykke.Service.RateCalculator.Client;
using Lykke.Service.RateCalculator.Client.AutorestClient.Models;
using AssetPair = Lykke.Service.Assets.Client.Models.AssetPair;

namespace Lykke.Service.MarketMakerReports.Services
{
    public class QuoteService : IQuoteService
    {
        private const int MarketMakerProfileLiveTimeSeconds = 1;
        
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        
        private readonly IAssetsServiceWithCache _assetsServiceWithCache;
        private readonly IRateCalculatorClient _rateCalculatorClient;
        
        private DateTime _marketProfileDate;
        private MarketProfile _marketProfile;
        
        public QuoteService(
            IAssetsServiceWithCache assetsServiceWithCache,
            IRateCalculatorClient rateCalculatorClient)
        {
            _assetsServiceWithCache = assetsServiceWithCache;
            _rateCalculatorClient = rateCalculatorClient;
        }
        
        public async Task<Quote> GetAsync(string baseAssetId, string quoteAssetId)
        {
            MarketProfile marketProfile = await GetMarketProfileAsync();
            
            string directAssetPairId = $"{baseAssetId}{quoteAssetId}";

            if (baseAssetId == quoteAssetId)
                return new Quote(directAssetPairId, DateTime.UtcNow, 1, 1);

            string assetPairId = directAssetPairId;

            bool inverted = false;

            AssetPair assetPair = await _assetsServiceWithCache.TryGetAssetPairAsync(assetPairId);

            if (assetPair == null)
            {
                assetPairId = $"{quoteAssetId}{baseAssetId}";
                inverted = true;

                assetPair = await _assetsServiceWithCache.TryGetAssetPairAsync(assetPairId);
            }

            if (assetPair == null)
            {
                throw new InvalidOperationException(
                    $"Asset pair does not exist for '{baseAssetId}'/'{quoteAssetId}'");
            }

            FeedData feedData = marketProfile.Profile.FirstOrDefault(o => o.Asset == assetPairId);

            if (feedData == null)
                throw new InvalidOperationException($"No quote for asset pair '{assetPairId}'");

            if (inverted)
            {
                return new Quote(directAssetPairId, feedData.DateTime, 1 / (decimal) feedData.Ask,
                    1 / (decimal) feedData.Bid);
            }

            return new Quote(directAssetPairId, feedData.DateTime, (decimal) feedData.Ask,
                (decimal) feedData.Bid);
        }

        private async Task<MarketProfile> GetMarketProfileAsync()
        {
            if ((DateTime.UtcNow - _marketProfileDate).TotalSeconds > MarketMakerProfileLiveTimeSeconds)
            {
                await _semaphore.WaitAsync();
                
                try
                {
                    if ((DateTime.UtcNow - _marketProfileDate).TotalSeconds > MarketMakerProfileLiveTimeSeconds)
                    {
                        _marketProfile = await _rateCalculatorClient.GetMarketProfileAsync();
                        _marketProfileDate = DateTime.UtcNow;
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }

            return _marketProfile;
        }
    }
}
