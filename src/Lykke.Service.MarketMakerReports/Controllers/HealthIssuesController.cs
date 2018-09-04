using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.MarketMakerReports.Client.Api;
using Lykke.Service.MarketMakerReports.Client.Models.Health;
using Lykke.Service.MarketMakerReports.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.MarketMakerReports.Controllers
{
    [Route("/api/[controller]")]
    public class HealthIssuesController : Controller, IHealthIssuesApi
    {
        private readonly IHealthMonitorService _healthMonitorService;

        public HealthIssuesController(IHealthMonitorService healthMonitorService)
        {
            _healthMonitorService = healthMonitorService;
        }
        
        /// <response code="200">Health issues</response>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<HealthIssueModel>), (int) HttpStatusCode.OK)]
        public async Task<IReadOnlyList<HealthIssueModel>> GetAsync(DateTime startDate, DateTime endDate)
        {
            var healthIssues = await _healthMonitorService.GetAsync(startDate, endDate);
            var model = Mapper.Map<List<HealthIssueModel>>(healthIssues);
            return model;
        }
    }
}
