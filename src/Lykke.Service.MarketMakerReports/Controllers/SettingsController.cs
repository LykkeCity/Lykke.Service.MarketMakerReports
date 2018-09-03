using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.MarketMakerReports.Client.Api;
using Lykke.Service.MarketMakerReports.Client.Models.Settings;
using Lykke.Service.MarketMakerReports.Core.Domain.Settings;
using Lykke.Service.MarketMakerReports.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.MarketMakerReports.Controllers
{
    [Route("/api/[controller]")]
    public class SettingsController : Controller, ISettingsApi
    {
        private readonly IAssetRealisedPnLSettingsService _assetRealisedPnLSettingsService;

        public SettingsController(IAssetRealisedPnLSettingsService assetRealisedPnLSettingsService)
        {
            _assetRealisedPnLSettingsService = assetRealisedPnLSettingsService;
        }

        /// <response code="200">An asset realised PnL calculation settings.</response>
        [HttpGet("realisedpnl")]
        [ProducesResponseType(typeof(IReadOnlyList<AssetRealisedPnLSettingsModel>), (int)HttpStatusCode.OK)]
        public async Task<AssetRealisedPnLSettingsModel> GetRealisedPnLSettingsAsync()
        {
            AssetRealisedPnLSettings assetRealisedPnLSettings = await _assetRealisedPnLSettingsService.GetAsync();

            return Mapper.Map<AssetRealisedPnLSettingsModel>(assetRealisedPnLSettings);
        }
        
        /// <response code="204">An asset realised PnL calculation settings successfully updated.</response>
        [HttpPost("realisedpnl")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task UpdateRealisedPnLSettingsAsync(AssetRealisedPnLSettingsModel model)
        {
            var assetRealisedPnLSettings = Mapper.Map<AssetRealisedPnLSettings>(model);
            
            await _assetRealisedPnLSettingsService.SaveAsync(assetRealisedPnLSettings);
        }
    }
}
