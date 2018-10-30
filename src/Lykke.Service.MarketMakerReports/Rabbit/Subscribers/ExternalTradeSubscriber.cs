using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.MarketMakerReports.Core.Extensions;
using Lykke.Service.MarketMakerReports.Core.Services;
using Lykke.Service.MarketMakerReports.Managers;
using Lykke.Service.MarketMakerReports.Settings.ServiceSettings.Rabbit;
using Lykke.Service.NettingEngine.Contract.Trades;

namespace Lykke.Service.MarketMakerReports.Rabbit.Subscribers
{
    public class ExternalTradeSubscriber : IDisposable, IStartable, IStoppable
    {
        private readonly ILogFactory _logFactory;
        private readonly ExchangeSettings _settings;
        private readonly IExternalTradeService _externalTradeService;
        private readonly IRealisedPnLService _realisedPnLService;
        private readonly ILog _log;
    
        private RabbitMqSubscriber<ExternalTrade> _subscriber;

        public ExternalTradeSubscriber(ILogFactory logFactory,
            ExchangeSettings exchangeSettings,
            IExternalTradeService externalTradeService,
            IRealisedPnLService realisedPnLService)
        {
            _log = logFactory.CreateLog(this);
            _logFactory = logFactory;
            _settings = exchangeSettings;
            _externalTradeService = externalTradeService;
            _realisedPnLService = realisedPnLService;
        }

        public void Start()
        {
            if(!_settings.Enabled)
                return;

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
            var sw = new Stopwatch();
            
            try
            {
                var model = Mapper.Map<Core.Domain.Trades.ExternalTrade>(message);
                
                await Task.WhenAll(
                    _externalTradeService.HandleAsync(model),
                    _realisedPnLService.CalculateAsync(model));
            }
            catch (Exception exception)
            {
                _log.ErrorWithDetails(exception, "An error occurred during processing external trade", message);
            }
            finally
            {
                sw.Stop();

                _log.InfoWithDetails("External trade was received",
                    new
                    {
                        message,
                        sw.ElapsedMilliseconds
                    });
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
