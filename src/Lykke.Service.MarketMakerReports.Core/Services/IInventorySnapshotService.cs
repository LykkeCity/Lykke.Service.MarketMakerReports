using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots;

namespace Lykke.Service.MarketMakerReports.Core.Services
{
    public interface IInventorySnapshotService
    {
        Task HandleAsync(InventorySnapshot model);
        
        Task<IEnumerable<InventorySnapshot>> GetAsync(DateTime startDate, DateTime endDate);
    }
}
