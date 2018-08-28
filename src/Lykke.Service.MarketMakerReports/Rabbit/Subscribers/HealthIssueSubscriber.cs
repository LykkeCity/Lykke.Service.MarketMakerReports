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
using Lykke.Service.NettingEngine.Client.RabbitMq.Health;

namespace Lykke.Service.MarketMakerReports.Rabbit.Subscribers
{
    public class HealthIssueSubscriber: IDisposable, IStartable, IStoppable
    {
        private readonly ILogFactory _logFactory;
        private readonly ExchangeSettings _settings;
        private readonly IHealthMonitorService _healthMonitorService;
        private readonly ILog _log;
    
        private RabbitMqSubscriber<HealthIssueMessage> _subscriber;

        public HealthIssueSubscriber(ILogFactory logFactory,
            ExchangeSettings exchangeSettings,
            IHealthMonitorService healthMonitorService)
        {
            _log = logFactory.CreateLog(this);
            _logFactory = logFactory;
            _settings = exchangeSettings;
            _healthMonitorService = healthMonitorService;
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings
                .CreateForSubscriber(_settings.ConnectionString, _settings.Exchange, _settings.Queue);

            settings.DeadLetterExchangeName = null;
            settings.IsDurable = true;

            _subscriber = new RabbitMqSubscriber<HealthIssueMessage>(_logFactory, settings,
                    new ResilientErrorHandlingStrategy(_logFactory, settings, TimeSpan.FromSeconds(10)))
                .SetMessageDeserializer(new JsonMessageDeserializer<HealthIssueMessage>())
                .SetMessageReadStrategy(new MessageReadQueueStrategy())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
                .Start();
        }

        private async Task ProcessMessageAsync(HealthIssueMessage message)
        {
            try
            {
                var model = Mapper.Map<Core.Domain.Health.HealthIssue>(message);
                await _healthMonitorService.HandleAsync(model);
            }
            catch (Exception exception)
            {
                _log.Error(exception, "An error occurred during processing health issue message", message);
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
