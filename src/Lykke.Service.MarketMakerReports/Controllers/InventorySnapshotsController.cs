using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.MarketMakerReports.Client.Api;
using Lykke.Service.MarketMakerReports.Client.Models.InventorySnapshots;
using Lykke.Service.MarketMakerReports.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

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
        
        /// <response code="200">Inventory snapshot</response>
        [HttpGet]
        [SwaggerOperation("InventorySnapshotGet")]
        [ProducesResponseType(typeof(IReadOnlyList<InventorySnapshotModel>), (int) HttpStatusCode.OK)]
        public async Task<IReadOnlyList<InventorySnapshotModel>> Get(DateTime startDate, DateTime endDate)
        {
            var snapshots = await _inventorySnapshotService.GetAsync(startDate, endDate);
            var model = Mapper.Map<List<InventorySnapshotModel>>(snapshots);
            return model;
        }
    }
}
