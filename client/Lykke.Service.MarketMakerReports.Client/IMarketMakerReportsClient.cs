using JetBrains.Annotations;
using Lykke.Service.MarketMakerReports.Client.Api;

namespace Lykke.Service.MarketMakerReports.Client
{
    /// <summary>
    /// MarketMakerReports client interface.
    /// </summary>
    [PublicAPI]
    public interface IMarketMakerReportsClient
    {
        IAuditMessagesApi AuditMessagesApi { get; }
        
        IInventorySnapshotsApi InventorySnapshotsApi { get; }
        
        ILykkeTradesApi LykkeTradesApi { get; }
        
        IExternalTradesApi ExternalTradesApi { get; }
    }
}
