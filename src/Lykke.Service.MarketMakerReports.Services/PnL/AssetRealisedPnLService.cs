using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.Assets.Client;
using Lykke.Service.MarketMakerReports.Core.Consts;
using Lykke.Service.MarketMakerReports.Core.Domain;
using Lykke.Service.MarketMakerReports.Core.Domain.PnL;
using Lykke.Service.MarketMakerReports.Core.Domain.Settings;
using Lykke.Service.MarketMakerReports.Core.Domain.Trades;
using Lykke.Service.MarketMakerReports.Core.Repositories;
using Lykke.Service.MarketMakerReports.Core.Services;
using Lykke.Service.MarketMakerReports.Services.Math;
using Lykke.Service.RateCalculator.Client;
using Lykke.Service.RateCalculator.Client.AutorestClient.Models;
using AssetPair = Lykke.Service.Assets.Client.Models.AssetPair;

namespace Lykke.Service.MarketMakerReports.Services.PnL
{
    [UsedImplicitly]
    public class AssetRealisedPnLService : IAssetRealisedPnLService
    {
        private const string QuoteAssetId = "USD";

        private readonly IAssetRealisedPnLRepository _assetRealisedPnLRepository;
        private readonly IWalletSettingsService _walletSettingsService;
        private readonly IRateCalculatorClient _rateCalculatorClient;
        private readonly IAssetsServiceWithCache _assetsServiceWithCache;
        private readonly ILog _log;

        private readonly AssetRealisedPnLCache _cache = new AssetRealisedPnLCache();

        public AssetRealisedPnLService(
            IAssetRealisedPnLRepository assetRealisedPnLRepository,
            IWalletSettingsService walletSettingsService,
            IRateCalculatorClient rateCalculatorClient,
            IAssetsServiceWithCache assetsServiceWithCache,
            ILogFactory logFactory)
        {
            _assetRealisedPnLRepository = assetRealisedPnLRepository;
            _walletSettingsService = walletSettingsService;
            _rateCalculatorClient = rateCalculatorClient;
            _assetsServiceWithCache = assetsServiceWithCache;
            _log = logFactory.CreateLog(this);
        }

        public async Task<IReadOnlyCollection<AssetRealisedPnL>> GetLastAsync(string walletId)
        {
            IReadOnlyCollection<AssetRealisedPnL> assetRealisedPnLs = _cache.Get(walletId);

            if (assetRealisedPnLs == null)
            {
                WalletSettings walletSettings = await _walletSettingsService.GetWalletAsync(walletId);

                if (walletSettings == null)
                    return new AssetRealisedPnL[0];
                
                Task<AssetRealisedPnL>[] tasks = walletSettings.Assets
                    .Select(assetId => _assetRealisedPnLRepository.GetLastAsync(walletId, assetId))
                    .ToArray();

                await Task.WhenAll(tasks);

                assetRealisedPnLs = tasks.Select(task => task.Result).ToArray();

                foreach (AssetRealisedPnL assetRealisedPnL in assetRealisedPnLs)
                    _cache.Initialize(assetRealisedPnL);
                
                assetRealisedPnLs = _cache.Get(walletId);
            }

            return assetRealisedPnLs;
        }

        public Task<IReadOnlyCollection<AssetRealisedPnL>> GetByAssetAsync(string walletId, string assetId,
            DateTime date, int? limit)
        {
            return _assetRealisedPnLRepository.GetAsync(walletId, assetId, date, limit);
        }

        public async Task CalculateAsync(LykkeTrade lykkeTrade)
        {
            WalletSettings walletSettings = await _walletSettingsService.GetWalletAsync(lykkeTrade.ClientId);

            if (walletSettings == null || !walletSettings.Enabled)
                return;

            AssetPair assetPair = await _assetsServiceWithCache.TryGetAssetPairAsync(lykkeTrade.AssetPairId);

            string[] assets = walletSettings.Assets
                .Intersect(new[] {assetPair.BaseAssetId, assetPair.QuotingAssetId})
                .ToArray();
           
            if(!assets.Any())
                return;
            
            var tradeData = new TradeData
            {
                Id = Guid.Empty.ToString("D"),
                Exchange = ExchangeNames.Lykke,
                AssetPair = assetPair.Id,
                BaseAsset = assetPair.BaseAssetId,
                QuoteAsset = assetPair.QuotingAssetId,
                Price = lykkeTrade.Price,
                Volume = lykkeTrade.Volume,
                Type = lykkeTrade.Type,
                Time = lykkeTrade.Time,
                LimitOrderId = lykkeTrade.LimitOrderId,
                OppositeClientId = lykkeTrade.OppositeClientId,
                OppositeLimitOrderId = lykkeTrade.OppositeLimitOrderId
            };
            
            MarketProfile marketProfile = await _rateCalculatorClient.GetMarketProfileAsync();
            
            foreach (string assetId in assets)
            {
                AssetRealisedPnL assetRealisedPnL =
                    await CalculateAsync(tradeData, walletSettings.Id, assetId, marketProfile);

                await _assetRealisedPnLRepository.InsertAsync(assetRealisedPnL);

                _cache.Set(assetRealisedPnL);
            }
        }

        public async Task CalculateAsync(ExternalTrade externalTrade)
        {
            IReadOnlyCollection<WalletSettings> walletsSettings = await _walletSettingsService.GetWalletsAsync();

            walletsSettings = walletsSettings.Where(o => o.Enabled && o.HandleExternalTrades).ToArray();
            
            if(!walletsSettings.Any())
                return;
            
            var tradeData = new TradeData
            {
                Id = externalTrade.OrderId,
                Exchange = externalTrade.Exchange,
                AssetPair = externalTrade.AssetPairId,
                BaseAsset = externalTrade.BaseAssetId,
                QuoteAsset = externalTrade.QuoteAssetId,
                Price = externalTrade.Price,
                Volume = externalTrade.Volume,
                Type = externalTrade.Type,
                Time = externalTrade.Time,
                LimitOrderId = externalTrade.OrderId,
                OppositeClientId = null,
                OppositeLimitOrderId = null
            };
            
            MarketProfile marketProfile = await _rateCalculatorClient.GetMarketProfileAsync();
            
            foreach (WalletSettings walletSettings in walletsSettings)
            {
                string[] assets = walletSettings.Assets
                    .Intersect(new[] {externalTrade.BaseAssetId, externalTrade.QuoteAssetId})
                    .ToArray();

                foreach (string assetId in assets)
                {
                    AssetRealisedPnL assetRealisedPnL =
                        await CalculateAsync(tradeData, walletSettings.Id, assetId, marketProfile);

                    await _assetRealisedPnLRepository.InsertAsync(assetRealisedPnL);

                    _cache.Set(assetRealisedPnL);
                }
            }
        }
        
        public async Task InitializeAsync(string walletId, string assetId, double amount)
        {
            MarketProfile marketProfile = await _rateCalculatorClient.GetMarketProfileAsync();
            
            Quote quote = await GetQuoteAsync(marketProfile, assetId, QuoteAssetId);
            
            var tradeData = new TradeData
            {
                Id = Guid.Empty.ToString("D"),
                Exchange = ExchangeNames.Lykke,
                AssetPair = $"{assetId}{QuoteAssetId}",
                BaseAsset = assetId,
                QuoteAsset = QuoteAssetId,
                Price = quote.Mid,
                Volume = (decimal) amount,
                Type = amount < 0 ? TradeType.Sell : TradeType.Buy,
                Time = DateTime.UtcNow,
                LimitOrderId = Guid.Empty.ToString("D"),
                OppositeClientId = null,
                OppositeLimitOrderId = null
            };
            
            AssetRealisedPnL assetRealisedPnL = await CalculateAsync(tradeData, walletId, assetId, marketProfile);

            await _assetRealisedPnLRepository.InsertAsync(assetRealisedPnL);
            
            _cache.Set(assetRealisedPnL);
        }

        private async Task<AssetRealisedPnL> CalculateAsync(TradeData tradeData, string walletId, string assetId, MarketProfile marketProfile)
        {
            AssetRealisedPnL prevAssetPnL = await _assetRealisedPnLRepository.GetLastAsync(walletId, assetId) ??
                                            new AssetRealisedPnL();

            bool inverted = tradeData.QuoteAsset == assetId;

            string crossAssetId = inverted
                ? tradeData.BaseAsset
                : tradeData.QuoteAsset;

            Quote quote = await GetQuoteAsync(marketProfile, assetId, QuoteAssetId);
            Quote crossQuote = await GetQuoteAsync(marketProfile, crossAssetId, QuoteAssetId);

            decimal volume = inverted
                ? tradeData.Price * tradeData.Volume
                : tradeData.Volume;

            decimal oppositeVolume = inverted
                ? tradeData.Volume * crossQuote.Mid
                : tradeData.Price * tradeData.Volume * crossQuote.Mid;

            RealisedPnLResult realisedPnLResult = Math.PnL.CalculateRealisedPnl(
                volume,
                oppositeVolume,
                prevAssetPnL.AvgPrice,
                prevAssetPnL.CumulativeVolume,
                prevAssetPnL.CumulativeOppositeVolume,
                prevAssetPnL.AvgPrice,
                quote.Mid,
                tradeData.Type == TradeType.Sell ? -1 : 1);

            decimal avgPrice = realisedPnLResult.CumulativeVolume != 0
                ? realisedPnLResult.CumulativeOppositeVolume / realisedPnLResult.CumulativeVolume
                : 0;

            decimal cumulativeRealisedPnL = prevAssetPnL.CumulativeRealisedPnL + realisedPnLResult.RealisedPnL;
            decimal unrealisedPnL = (quote.Mid - avgPrice) * realisedPnLResult.CumulativeVolume;

            return new AssetRealisedPnL
            {
                WalletId = walletId,
                AssetId = assetId,
                Time = tradeData.Time,
                Exchange = tradeData.Exchange,

                TradeId = tradeData.Id,
                TradeAssetPair = tradeData.AssetPair,
                TradePrice = tradeData.Price,
                TradeVolume = tradeData.Volume,
                TradeType = tradeData.Type,

                CrossAssetPair = crossQuote.AssetPair,
                CrossPrice = crossQuote.Mid,

                Price = quote.Mid,
                Volume = volume,
                OppositeVolume = oppositeVolume,
                Inverted = inverted,

                PrevAvgPrice = prevAssetPnL.AvgPrice,
                PrevCumulativeVolume = prevAssetPnL.CumulativeVolume,
                PrevCumulativeOppositeVolume = prevAssetPnL.CumulativeOppositeVolume,

                CloseVolume = realisedPnLResult.ClosedVolume,
                RealisedPnL = realisedPnLResult.RealisedPnL,

                AvgPrice = avgPrice,
                CumulativeVolume = realisedPnLResult.CumulativeVolume,
                CumulativeOppositeVolume = realisedPnLResult.CumulativeOppositeVolume,
                CumulativeRealisedPnL = cumulativeRealisedPnL,
                UnrealisedPnL = unrealisedPnL,
                
                LimitOrderId = tradeData.LimitOrderId,
                OppositeClientId = tradeData.OppositeClientId,
                OppositeLimitOrderId = tradeData.OppositeLimitOrderId
            };
        }

        private async Task<Quote> GetQuoteAsync(MarketProfile marketProfile, string baseAssetId, string quoteAssetId)
        {
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

            decimal rate = 1;

            if (inverted)
                rate = 1 / (((decimal) feedData.Ask + (decimal) feedData.Bid) / 2m);

            return new Quote(directAssetPairId, feedData.DateTime, (decimal) feedData.Ask * rate,
                (decimal) feedData.Bid * rate);
        }
    }
}
