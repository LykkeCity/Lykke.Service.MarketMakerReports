using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots;

namespace Lykke.Service.MarketMakerReports.Core.Repositories
{
    public interface IInventorySnapshotRepository
    {
        Task InsertAsync(InventorySnapshot inventorySnapshot);

        Task<IEnumerable<InventorySnapshot>> GetAsync(DateTime startDate, DateTime endDate, Periodicity periodicity);

        Task<InventorySnapshot> GetLastForDateAsync(DateTime date);
        
        Task<InventorySnapshot> GetFirstForDateAsync(DateTime date);
    }
}
