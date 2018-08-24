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
    public class ExternalTradesController : Controller, IExternalTradesApi
    {
        private readonly IExternalTradeService _externalTradeService;

        public ExternalTradesController(IExternalTradeService externalTradeService)
        {
            _externalTradeService = externalTradeService;
        }
        
        /// <response code="200">External trades</response>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<ExternalTradeModel>), (int) HttpStatusCode.OK)]
        public async Task<IReadOnlyList<ExternalTradeModel>> GetAsync(DateTime startDate, DateTime endDate)
        {
            var trades = await _externalTradeService.GetAsync(startDate, endDate);
            var model = Mapper.Map<List<ExternalTradeModel>>(trades);
            return model;
        }
    }
}
