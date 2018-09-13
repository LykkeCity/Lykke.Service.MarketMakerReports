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
        Task<IReadOnlyList<InventorySnapshotModel>> GetAsync(DateTime startDate, DateTime endDate,
            Periodicity periodicity);

        [Get("/api/inventorysnapshots/assetdynamics")]
        Task<AssetInventoryDynamicsModel> GetAssetDynamicsAsync(DateTime startDate, DateTime endDate);

        [Get("/api/inventorysnapshots/assetpairdynamics")]
        Task<AssetPairInventoryDynamicsModel> GetAssetPairDynamicsAsync(DateTime startDate, DateTime endDate);

        [Get("/api/inventorysnapshots/last")]
        Task<InventorySnapshotModel> GetLastAsync();

        [Get("/api/inventorysnapshots/timeline")]
        Task<IReadOnlyList<InventorySnapshotBriefModel>> GetTimelineAsync(DateTime startDate, DateTime endDate,
            Periodicity periodicity);
    }
}
