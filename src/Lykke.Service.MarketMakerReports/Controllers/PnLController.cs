using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.MarketMakerReports.Client.Api;
using Lykke.Service.MarketMakerReports.Client.Models.PnL;
using Lykke.Service.MarketMakerReports.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.MarketMakerReports.Controllers
{
    [Route("/api/[controller]")]
    public class PnLController : Controller, IPnLApi
    {
        private readonly IPnLService _pnLService;

        public PnLController(IPnLService pnLService)
        {
            _pnLService = pnLService;
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
    }
}
