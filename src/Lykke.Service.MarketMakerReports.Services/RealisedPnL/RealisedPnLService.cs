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
using Lykke.Service.MarketMakerReports.Core.Extensions;
using Lykke.Service.MarketMakerReports.Core.Repositories;
using Lykke.Service.MarketMakerReports.Core.Services;
using AssetPair = Lykke.Service.Assets.Client.Models.AssetPair;

namespace Lykke.Service.MarketMakerReports.Services.RealisedPnL
{
    [UsedImplicitly]
    public class RealisedPnLService : IRealisedPnLService
    {
        private const string QuoteAssetId = "USD";

        private readonly IAssetRealisedPnLRepository _assetRealisedPnLRepository;
        private readonly IWalletSettingsService _walletSettingsService;
        private readonly IAssetsServiceWithCache _assetsServiceWithCache;
        private readonly IQuoteService _quoteService;
        private readonly ILog _log;

        private readonly RealisedPnLCache _cache;

        public RealisedPnLService(
            IAssetRealisedPnLRepository assetRealisedPnLRepository,
            IWalletSettingsService walletSettingsService,
            IAssetsServiceWithCache assetsServiceWithCache,
            IQuoteService quoteService,
            ILogFactory logFactory)
        {
            _assetRealisedPnLRepository = assetRealisedPnLRepository;
            _walletSettingsService = walletSettingsService;
            _assetsServiceWithCache = assetsServiceWithCache;
            _quoteService = quoteService;
            _log = logFactory.CreateLog(this);
            
            _cache = new RealisedPnLCache();
        }

        public async Task<IReadOnlyCollection<AssetRealisedPnL>> GetLastAsync(string walletId)
        {
            IReadOnlyCollection<AssetRealisedPnL> assetsRealisedPnL = _cache.Get(walletId);

            if (assetsRealisedPnL == null)
            {
                WalletSettings walletSettings = await _walletSettingsService.GetWalletAsync(walletId);

                if (walletSettings == null)
                    return new AssetRealisedPnL[0];

                Task<AssetRealisedPnL>[] tasks = walletSettings.Assets
                    .Select(assetId => _assetRealisedPnLRepository.GetLastAsync(walletId, assetId))
                    .ToArray();

                await Task.WhenAll(tasks);

                assetsRealisedPnL = tasks.Where(task => task.Result != null)
                    .Select(task => task.Result)
                    .ToArray();

                _cache.Initialize(walletId, assetsRealisedPnL);

                assetsRealisedPnL = _cache.Get(walletId);
            }

            return assetsRealisedPnL;
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

            if (!assets.Any())
                return;

            var tradeData = new TradeData
            {
                Id = lykkeTrade.Id,
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

            foreach (string assetId in assets)
            {
                AssetRealisedPnL assetRealisedPnL =
                    await CalculateAsync(tradeData, walletSettings.Id, assetId);

                await _assetRealisedPnLRepository.InsertAsync(assetRealisedPnL);

                _cache.Set(assetRealisedPnL);
            }

            _log.InfoWithDetails("Lykke trade handled", tradeData);
        }

        public async Task CalculateAsync(ExternalTrade externalTrade)
        {
            IReadOnlyCollection<WalletSettings> walletsSettings = await _walletSettingsService.GetWalletsAsync();

            walletsSettings = walletsSettings.Where(o => o.Enabled && o.HandleExternalTrades).ToArray();

            if (!walletsSettings.Any())
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

            foreach (WalletSettings walletSettings in walletsSettings)
            {
                string[] assets = walletSettings.Assets
                    .Intersect(new[] {externalTrade.BaseAssetId, externalTrade.QuoteAssetId})
                    .ToArray();

                foreach (string assetId in assets)
                {
                    AssetRealisedPnL assetRealisedPnL =
                        await CalculateAsync(tradeData, walletSettings.Id, assetId);

                    await _assetRealisedPnLRepository.InsertAsync(assetRealisedPnL);

                    _cache.Set(assetRealisedPnL);
                }
            }

            _log.InfoWithDetails("External trade handled", tradeData);
        }

        public async Task InitializeAsync(string walletId, string assetId, double amount)
        {
            if (Math.Abs(amount) <= Double.Epsilon)
                return;

            Quote quote = await _quoteService.GetAsync(assetId, QuoteAssetId);

            var tradeData = new TradeData
            {
                Id = Guid.Empty.ToString("D"),
                Exchange = ExchangeNames.Lykke,
                AssetPair = $"{assetId}{QuoteAssetId}",
                BaseAsset = assetId,
                QuoteAsset = QuoteAssetId,
                Price = quote.Mid,
                Volume = (decimal) Math.Abs(amount),
                Type = amount < 0 ? TradeType.Sell : TradeType.Buy,
                Time = DateTime.UtcNow,
                LimitOrderId = Guid.Empty.ToString("D"),
                OppositeClientId = null,
                OppositeLimitOrderId = null
            };

            AssetRealisedPnL assetRealisedPnL = await CalculateAsync(tradeData, walletId, assetId);

            await _assetRealisedPnLRepository.InsertAsync(assetRealisedPnL);

            _log.InfoWithDetails("Realised PnL initialized", new {walletId, assetId, amount});

            _cache.Set(assetRealisedPnL);
        }

        private async Task<AssetRealisedPnL> CalculateAsync(TradeData tradeData, string walletId, string assetId)
        {
            AssetRealisedPnL prevAssetPnL = await _assetRealisedPnLRepository.GetLastAsync(walletId, assetId) ??
                                            new AssetRealisedPnL();
            
            bool inverted = tradeData.QuoteAsset == assetId;

            string crossAssetId = inverted
                ? tradeData.BaseAsset
                : tradeData.QuoteAsset;

            Quote quote = await _quoteService.GetAsync(assetId, QuoteAssetId);
            Quote crossQuote = await _quoteService.GetAsync(crossAssetId, QuoteAssetId);

            RealisedPnLResult realisedPnLResult = RealisedPnLCalculator.Calculate(
                tradeData.Price,
                tradeData.Volume,
                inverted,
                tradeData.Type == TradeType.Sell ? -1 : 1,
                prevAssetPnL.CumulativeVolume,
                prevAssetPnL.CumulativeOppositeVolume,
                quote.Mid,
                prevAssetPnL.AvgPrice,
                crossQuote.Mid);
                
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

                Price = realisedPnLResult.Price,
                Volume = realisedPnLResult.Volume,
                OppositeVolume = realisedPnLResult.OppositeVolume,
                Inverted = inverted,

                PrevAvgPrice = prevAssetPnL.AvgPrice,
                PrevCumulativeVolume = prevAssetPnL.CumulativeVolume,
                PrevCumulativeOppositeVolume = prevAssetPnL.CumulativeOppositeVolume,

                OpenPrice = prevAssetPnL.AvgPrice,
                ClosePrice = realisedPnLResult.Price,
                CloseVolume = realisedPnLResult.ClosedVolume,
                RealisedPnL = realisedPnLResult.RealisedPnL,

                AvgPrice = realisedPnLResult.AvgPrice,
                CumulativeVolume = realisedPnLResult.CumulativeVolume,
                CumulativeOppositeVolume = realisedPnLResult.CumulativeOppositeVolume,
                CumulativeRealisedPnL = prevAssetPnL.CumulativeRealisedPnL + realisedPnLResult.RealisedPnL,
                
                Rate = quote.Mid,
                UnrealisedPnL = realisedPnLResult.UnrealisedPnL,

                LimitOrderId = tradeData.LimitOrderId,
                OppositeClientId = tradeData.OppositeClientId,
                OppositeLimitOrderId = tradeData.OppositeLimitOrderId
            };
        }
    }
}
