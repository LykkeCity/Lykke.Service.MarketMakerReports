using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.MarketMakerReports.Client.Api;
using Lykke.Service.MarketMakerReports.Client.Models.InventorySnapshots;
using Lykke.Service.MarketMakerReports.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.MarketMakerReports.Controllers
{
    [Route("/api/[controller]")]
    public class InventorySnapshotsController : Controller, IInventorySnapshotsApi
    {
        private readonly IInventorySnapshotService _inventorySnapshotService;

        public InventorySnapshotsController(IInventorySnapshotService inventorySnapshotService)
        {
            _inventorySnapshotService = inventorySnapshotService;
        }
        
        /// <response code="200">Inventory snapshots</response>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<InventorySnapshotModel>), (int) HttpStatusCode.OK)]
        public async Task<IReadOnlyList<InventorySnapshotModel>> GetAsync(DateTime startDate, DateTime endDate, Periodicity periodicity)
        {
            var snapshots = await _inventorySnapshotService.GetAsync(startDate, endDate, (Core.Domain.InventorySnapshots.Periodicity)periodicity);
            var model = Mapper.Map<List<InventorySnapshotModel>>(snapshots);
            return model;
        }
        
        /// <response code="200">Asset inventory dynamics</response>
        [HttpGet("assetdynamics")]
        [ProducesResponseType(typeof(AssetInventoryDynamicsModel), (int)HttpStatusCode.OK)]
        public async Task<AssetInventoryDynamicsModel> GetAssetDynamicsAsync(DateTime startDate, DateTime endDate)
        {
            var dynamics = await _inventorySnapshotService.GetDynamicsAsync(startDate, endDate);
            var model = Mapper.Map<AssetInventoryDynamicsModel>(dynamics);
            return model;
        }

        /// <response code="200">Asset pairs inventory dynamics</response>
        [HttpGet("assetpairdynamics")]
        [ProducesResponseType(typeof(AssetPairInventoryDynamicsModel), (int)HttpStatusCode.OK)]
        public async Task<AssetPairInventoryDynamicsModel> GetAssetPairDynamicsAsync(DateTime startDate, DateTime endDate)
        {
            var dynamics = await _inventorySnapshotService.GetDynamicsAsync(startDate, endDate);
            var model = Mapper.Map<AssetPairInventoryDynamicsModel>(dynamics);
            return model;
        }

        /// <response code="200">The last inventory snapshot</response>
        [HttpGet("last")]
        [ProducesResponseType(typeof(IReadOnlyList<InventorySnapshotModel>), (int) HttpStatusCode.OK)]
        public async Task<InventorySnapshotModel> GetLastAsync()
        {
            var snapshot = await _inventorySnapshotService.GetLastAsync();
            var model = Mapper.Map<InventorySnapshotModel>(snapshot);
            return model;
        }

        /// <response code="200">Inventory snapshots timeline</response>
        [HttpGet("timeline")]
        [ProducesResponseType(typeof(IReadOnlyList<InventorySnapshotModel>), (int)HttpStatusCode.OK)]
        public async Task<IReadOnlyList<InventorySnapshotBriefModel>> GetTimelineAsync(DateTime startDate, DateTime endDate, Periodicity periodicity)
        {
            var snapshots = await _inventorySnapshotService.GetAsync(startDate, endDate, (Core.Domain.InventorySnapshots.Periodicity)periodicity);
            var model = Mapper.Map<List<InventorySnapshotBriefModel>>(snapshots);
            return model;
        }
    }
}
