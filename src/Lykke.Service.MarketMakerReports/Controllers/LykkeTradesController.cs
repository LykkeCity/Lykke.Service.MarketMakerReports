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
        [ProducesResponseType(typeof(ContinuationTokenResult<LykkeTradeModel>), (int) HttpStatusCode.OK)]
        public async Task<ContinuationTokenResult<LykkeTradeModel>> GetAsync(DateTime startDate, DateTime endDate, 
            int? limit, string continuationToken)
        {
            var tradesWithToken = await _lykkeTradeService.GetAsync(startDate, endDate, limit, continuationToken);
            
            return new ContinuationTokenResult<LykkeTradeModel>
            {
                Entities = Mapper.Map<List<LykkeTradeModel>>(tradesWithToken.entities),
                ContinuationToken = tradesWithToken.continuationToken
            };
        }
    }
}
