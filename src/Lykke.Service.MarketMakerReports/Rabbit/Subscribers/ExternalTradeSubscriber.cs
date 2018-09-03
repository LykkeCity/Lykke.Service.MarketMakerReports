using System;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.MarketMakerReports.Core.Services;
using Lykke.Service.MarketMakerReports.Managers;
using Lykke.Service.MarketMakerReports.Settings.ServiceSettings;
using Lykke.Service.NettingEngine.Client.RabbitMq.Trades;

namespace Lykke.Service.MarketMakerReports.Rabbit.Subscribers
{
    public class ExternalTradeSubscriber : IDisposable, IStartable, IStoppable
    {
        private readonly ILogFactory _logFactory;
        private readonly ExchangeSettings _settings;
        private readonly IExternalTradeService _externalTradeService;
        private readonly IAssetRealisedPnLService _assetRealisedPnLService;
        private readonly ILog _log;
    
        private RabbitMqSubscriber<ExternalTrade> _subscriber;

        public ExternalTradeSubscriber(ILogFactory logFactory,
            ExchangeSettings exchangeSettings,
            IExternalTradeService externalTradeService,
            IAssetRealisedPnLService assetRealisedPnLService)
        {
            _log = logFactory.CreateLog(this);
            _logFactory = logFactory;
            _settings = exchangeSettings;
            _externalTradeService = externalTradeService;
            _assetRealisedPnLService = assetRealisedPnLService;
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings
                .CreateForSubscriber(_settings.ConnectionString, _settings.Exchange, _settings.Queue);

            settings.DeadLetterExchangeName = null;
            settings.IsDurable = true;

            _subscriber = new RabbitMqSubscriber<ExternalTrade>(_logFactory, settings,
                    new ResilientErrorHandlingStrategy(_logFactory, settings, TimeSpan.FromSeconds(10)))
                .SetMessageDeserializer(new JsonMessageDeserializer<ExternalTrade>())
                .SetMessageReadStrategy(new MessageReadQueueStrategy())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
                .Start();
        }

        private async Task ProcessMessageAsync(ExternalTrade message)
        {
            try
            {
                var model = Mapper.Map<Core.Domain.Trades.ExternalTrade>(message);
                await _externalTradeService.HandleAsync(model);
                await _assetRealisedPnLService.CalculateAsync(model);
            }
            catch (Exception exception)
            {
                _log.Error(exception, "An error occurred during processing external trade message", message);
            }
        }

        public void Stop()
        {
            _subscriber?.Stop();
        }

        public void Dispose()
        {
            _subscriber?.Dispose();
        }
    }
}
