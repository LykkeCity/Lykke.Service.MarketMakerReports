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
    public class LykkeTradeSubscriber : IDisposable, IStartable, IStoppable
    {
        private readonly ILogFactory _logFactory;
        private readonly ExchangeSettings _settings;
        private readonly ILykkeTradeService _lykkeTradeService;
        private readonly IRealisedPnLService _realisedPnLService;
        private readonly ILog _log;

        private RabbitMqSubscriber<LykkeTrade> _subscriber;

        public LykkeTradeSubscriber(ILogFactory logFactory,
            ExchangeSettings exchangeSettings,
            ILykkeTradeService lykkeTradeService,
            IRealisedPnLService realisedPnLService)
        {
            _log = logFactory.CreateLog(this);
            _logFactory = logFactory;
            _settings = exchangeSettings;
            _lykkeTradeService = lykkeTradeService;
            _realisedPnLService = realisedPnLService;
        }

        public void Start()
        {
            if (!_settings.Enabled)
                return;

            var settings = RabbitMqSubscriptionSettings
                .CreateForSubscriber(_settings.ConnectionString, _settings.Exchange, _settings.Queue);

            settings.DeadLetterExchangeName = null;
            settings.IsDurable = true;

            _subscriber = new RabbitMqSubscriber<LykkeTrade>(_logFactory, settings,
                    new ResilientErrorHandlingStrategy(_logFactory, settings, TimeSpan.FromSeconds(10)))
                .SetMessageDeserializer(new JsonMessageDeserializer<LykkeTrade>())
                .SetMessageReadStrategy(new MessageReadQueueStrategy())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
                .Start();
        }

        private async Task ProcessMessageAsync(LykkeTrade message)
        {
            var sw = new Stopwatch();

            try
            {
                var model = Mapper.Map<Core.Domain.Trades.LykkeTrade>(message);

                await Task.WhenAll(
                    _lykkeTradeService.HandleAsync(model),
                    _realisedPnLService.CalculateAsync(model));
            }
            catch (Exception exception)
            {
                _log.ErrorWithDetails(exception, "An error occurred during processing Lykke trade", message);
            }
            finally
            {
                sw.Stop();

                _log.InfoWithDetails("Lykke trade was received",
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
