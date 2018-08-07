using System;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.MarketMakerReports.Core.Services;
using Lykke.Service.MarketMakerReports.Settings.ServiceSettings;
using Lykke.Service.NettingEngine.Client.RabbitMq.InventorySnapshots;

namespace Lykke.Service.MarketMakerReports.Rabbit.Subscribers
{
    public class InventorySnapshotSubscriber : IDisposable
    {
        private readonly ILogFactory _logFactory;
        private readonly ExchangeSettings _settings;
        private readonly IInventorySnapshotService _inventorySnapshotService;
        private readonly ILog _log;
    
        private RabbitMqSubscriber<InventorySnapshot> _subscriber;

        public InventorySnapshotSubscriber(ILogFactory logFactory,
            ExchangeSettings exchangeSettings,
            IInventorySnapshotService inventorySnapshotService)
        {
            _log = logFactory.CreateLog(this);
            _logFactory = logFactory;
            _settings = exchangeSettings;
            _inventorySnapshotService = inventorySnapshotService;
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings
                .CreateForSubscriber(_settings.ConnectionString, _settings.Exchange, _settings.Queue);

            settings.DeadLetterExchangeName = null;
            settings.IsDurable = true;

            _subscriber = new RabbitMqSubscriber<InventorySnapshot>(_logFactory, settings,
                    new ResilientErrorHandlingStrategy(_logFactory, settings, TimeSpan.FromSeconds(10)))
                .SetMessageDeserializer(new JsonMessageDeserializer<InventorySnapshot>())
                .SetMessageReadStrategy(new MessageReadQueueStrategy())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
                .Start();
        }

        private async Task ProcessMessageAsync(InventorySnapshot message)
        {
            try
            {
                var model = Mapper.Map<Core.Domain.InventorySnapshots.InventorySnapshot>(message);
                
                await _inventorySnapshotService.HandleAsync(model);
            }
            catch (Exception exception)
            {
                _log.Error(exception, "An error occurred during processing inventory snapshot", message);
            }
        }

        public void Stop()
        {
            _subscriber?.Stop();
        }
        
        public void Dispose()
        {
            _subscriber?.Stop();
        }
    }
}
