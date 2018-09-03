using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.MarketMakerReports.Client.Api;
using Lykke.Service.MarketMakerReports.Client.Models.PnL;
using Lykke.Service.MarketMakerReports.Core.Domain.PnL;
using Lykke.Service.MarketMakerReports.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.MarketMakerReports.Controllers
{
    [Route("/api/[controller]")]
    public class PnLController : Controller, IPnLApi
    {
        private readonly IPnLService _pnLService;
        private readonly IAssetRealisedPnLService _assetRealisedPnLService;

        public PnLController(IPnLService pnLService, IAssetRealisedPnLService assetRealisedPnLService)
        {
            _pnLService = pnLService;
            _assetRealisedPnLService = assetRealisedPnLService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PnLResultModel), (int)HttpStatusCode.OK)]
        public async Task<PnLResultModel> GetAsync(DateTime startDate, DateTime endDate)
        {
            var result = await _pnLService.GetPnLAsync(startDate, endDate);

            var model = Mapper.Map<PnLResultModel>(result);

            return model;
        }

        [HttpGet("currentday")]
        [ProducesResponseType(typeof(PnLResultModel), (int)HttpStatusCode.OK)]
        public async Task<PnLResultModel> GetForCurrentDayAsync()
        {
            var result = await _pnLService.GetCurrentDayPnLAsync();

            var model = Mapper.Map<PnLResultModel>(result);

            return model;
        }

        [HttpGet("currentmonth")]
        [ProducesResponseType(typeof(PnLResultModel), (int)HttpStatusCode.OK)]
        public async Task<PnLResultModel> GetForCurrentMonthAsync()
        {
            var result = await _pnLService.GetCurrentMonthPnLAsync();

            var model = Mapper.Map<PnLResultModel>(result);

            return model;
        }

        /// <response code="200">A collection of asset realised PnL records.</response>
        [HttpGet("realised")]
        [ProducesResponseType(typeof(IReadOnlyList<AssetRealisedPnLModel>), (int)HttpStatusCode.OK)]
        public async Task<IReadOnlyList<AssetRealisedPnLModel>> GetRealisedAsync(string assetId, int? limit)
        {
            IReadOnlyList<AssetRealisedPnL> assetRealisedPnL = await _assetRealisedPnLService.GetAsync(assetId, limit);

            var model = Mapper.Map<List<AssetRealisedPnLModel>>(assetRealisedPnL);

            return model;
        }
    }
}
