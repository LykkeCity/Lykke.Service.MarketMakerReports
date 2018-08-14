using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Refit;
using Lykke.Service.MarketMakerReports.Client.Models.InventorySnapshots;

namespace Lykke.Service.MarketMakerReports.Client.Api
{
    [PublicAPI]
    public interface IInventorySnapshotsApi
    {
        [Get("/api/inventorysnapshots")]
        Task<IReadOnlyList<InventorySnapshotModel>> Get(DateTime startDate, DateTime endDate, Periodicity periodicity);

        [Get("/api/inventorysnapshots/last")]
        Task<InventorySnapshotModel> GetLast();
    }
}
