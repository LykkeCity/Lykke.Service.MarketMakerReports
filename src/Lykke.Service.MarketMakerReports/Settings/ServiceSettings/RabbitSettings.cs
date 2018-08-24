namespace Lykke.Service.MarketMakerReports.Settings.ServiceSettings
{
    public class RabbitSettings
    {
        public ExchangeSettings AuditMessage { get; set; }
        
        public ExchangeSettings InventorySnapshot { get; set; }
        
        public ExchangeSettings LykkeTrade { get; set; }
        
        public ExchangeSettings ExternalTrade { get; set; }
    }
}
