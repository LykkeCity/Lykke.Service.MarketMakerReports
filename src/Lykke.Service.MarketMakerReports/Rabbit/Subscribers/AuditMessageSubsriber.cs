using System;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.MarketMakerReports.Core.Services;
using Lykke.Service.MarketMakerReports.Managers;
using Lykke.Service.MarketMakerReports.Settings.ServiceSettings.Rabbit;
using Lykke.Service.NettingEngine.Client.RabbitMq;

namespace Lykke.Service.MarketMakerReports.Rabbit.Subscribers
{
    [UsedImplicitly]
    public class AuditMessageSubscriber : IDisposable, IStartable, IStoppable
    {
        private readonly ILogFactory _logFactory;
        private readonly ExchangeSettings _settings;
        private readonly IAuditMessageService _auditMessageService;
        private readonly ILog _log;
    
        private RabbitMqSubscriber<AuditMessage> _subscriber;

        public AuditMessageSubscriber(ILogFactory logFactory,
            ExchangeSettings exchangeSettings,
            IAuditMessageService auditMessageService)
        {
            _log = logFactory.CreateLog(this);
            _logFactory = logFactory;
            _settings = exchangeSettings;
            _auditMessageService = auditMessageService;
        }

        public void Start()
        {
            if (!_settings.Enabled)
                return;
            
            var settings = RabbitMqSubscriptionSettings
                .CreateForSubscriber(_settings.ConnectionString, _settings.Exchange, _settings.Queue);

            settings.DeadLetterExchangeName = null;
            settings.IsDurable = true;

            _subscriber = new RabbitMqSubscriber<AuditMessage>(_logFactory, settings,
                    new ResilientErrorHandlingStrategy(_logFactory, settings, TimeSpan.FromSeconds(10)))
                .SetMessageDeserializer(new JsonMessageDeserializer<AuditMessage>())
                .SetMessageReadStrategy(new MessageReadQueueStrategy())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
                .Start();
        }

        private async Task ProcessMessageAsync(AuditMessage message)
        {
            try
            {
                var model = Mapper.Map<Core.Domain.AuditMessages.AuditMessage>(message);
                
                await _auditMessageService.HandleAsync(model);
            }
            catch (Exception exception)
            {
                _log.Error(exception, "An error occurred during processing audit message", message);
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
