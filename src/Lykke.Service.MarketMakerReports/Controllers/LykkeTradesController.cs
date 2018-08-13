using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.MarketMakerReports.Client.Api;
using Lykke.Service.MarketMakerReports.Client.Models.Trades;
using Lykke.Service.MarketMakerReports.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.MarketMakerReports.Controllers
{
    [Route("/api/[controller]")]
    public class LykkeTradesController : Controller, ILykkeTradesApi
    {
        private readonly ILykkeTradeService _lykkeTradeService;

        public LykkeTradesController(ILykkeTradeService lykkeTradeService)
        {
            _lykkeTradeService = lykkeTradeService;
        }
        
        /// <response code="200">Lykke trades</response>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<LykkeTradeModel>), (int) HttpStatusCode.OK)]
        public async Task<IReadOnlyList<LykkeTradeModel>> Get(DateTime startDate, DateTime endDate)
        {
            var trades = await _lykkeTradeService.GetAsync(startDate, endDate);
            var model = Mapper.Map<List<LykkeTradeModel>>(trades);
            return model;
        }
    }
}
