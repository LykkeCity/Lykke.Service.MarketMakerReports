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
        [ProducesResponseType(typeof(ContinuationTokenResult<ExternalTradeModel>), (int) HttpStatusCode.OK)]
        public async Task<ContinuationTokenResult<ExternalTradeModel>> GetAsync(DateTime startDate, DateTime endDate, 
            int? limit, string continuationToken)
        {
            var tradesWithToken = await _externalTradeService.GetAsync(startDate, endDate, limit, continuationToken);
            
            return new ContinuationTokenResult<ExternalTradeModel>
            {
                Entities = Mapper.Map<List<ExternalTradeModel>>(tradesWithToken.entities),
                ContinuationToken = tradesWithToken.continuationToken
            };
        }
    }
}
