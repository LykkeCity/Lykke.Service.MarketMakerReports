namespace Lykke.Service.MarketMakerReports.Settings.ServiceSettings
{
    public class RabbitSettings
    {
        public ExchangeSettings AuditMessage { get; set; }
        
        public ExchangeSettings InventorySnapshot { get; set; }
    }
}