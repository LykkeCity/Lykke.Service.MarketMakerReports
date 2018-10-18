using JetBrains.Annotations;

namespace Lykke.Service.MarketMakerReports.Settings.ServiceSettings.Rabbit
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class RabbitSettings
    {
        public ExchangeSettings AuditMessage { get; set; }

        public ExchangeSettings InventorySnapshot { get; set; }

        public ExchangeSettings LykkeTrade { get; set; }

        public ExchangeSettings ExternalTrade { get; set; }

        public ExchangeSettings HealthIssue { get; set; }
    }
}
