using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.MarketMakerReports.Client.Api;
using Lykke.Service.MarketMakerReports.Client.Models.RealisedPnLSettings;
using Lykke.Service.MarketMakerReports.Core.Domain.Settings;
using Lykke.Service.MarketMakerReports.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.MarketMakerReports.Controllers
{
    [Route("/api/[controller]")]
    public class RealisedPnLSettingsController : Controller, IRealisedPnLSettingsApi
    {
        private readonly IWalletSettingsService _walletSettingsService;
        private readonly IRealisedPnLService _realisedPnLService;

        public RealisedPnLSettingsController(
            IWalletSettingsService walletSettingsService,
            IRealisedPnLService realisedPnLService)
        {
            _walletSettingsService = walletSettingsService;
            _realisedPnLService = realisedPnLService;
        }

        /// <response code="200">A collection of settings of wallets.</response>
        [HttpGet("wallets")]
        [ProducesResponseType(typeof(IReadOnlyCollection<WalletSettingsModel>), (int) HttpStatusCode.OK)]
        public async Task<IReadOnlyCollection<WalletSettingsModel>> GetWalletsAsync()
        {
            IReadOnlyCollection<WalletSettings> walletSettings = await _walletSettingsService.GetWalletsAsync();

            return Mapper.Map<List<WalletSettingsModel>>(walletSettings);
        }

        /// <response code="200">A wallet settings.</response>
        [HttpGet("wallets/{walletId}")]
        [ProducesResponseType(typeof(WalletSettingsModel), (int) HttpStatusCode.OK)]
        public async Task<WalletSettingsModel> GetWalletAsync(string walletId)
        {
            WalletSettings walletSettings = await _walletSettingsService.GetWalletAsync(walletId);

            return Mapper.Map<WalletSettingsModel>(walletSettings);
        }

        /// <response code="204">A wallet settings successfully added.</response>
        [HttpPost("wallets")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public Task AddWalletAsync([FromBody] WalletSettingsModel walletSettingsModel)
        {
            var walletSettings = Mapper.Map<WalletSettings>(walletSettingsModel);

            return _walletSettingsService.AddWalletAsync(walletSettings);
        }

        /// <response code="204">An asset settings successfully added.</response>
        [HttpPost("assets")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task AddAssetToWalletAsync([FromBody] AssetSettingsModel assetSettingsModel)
        {
            await _walletSettingsService.AddAssetToWalletAsync(assetSettingsModel.WalletId, assetSettingsModel.Id);

            await _realisedPnLService.InitializeAsync(assetSettingsModel.WalletId, assetSettingsModel.Id,
                assetSettingsModel.Amount);
        }

        /// <response code="204">A wallet settings successfully updated.</response>
        [HttpPut("wallets")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public Task UpdateWalletAsync([FromBody] WalletSettingsModel walletSettingsModel)
        {
            var walletSettings = Mapper.Map<WalletSettings>(walletSettingsModel);

            return _walletSettingsService.UpdateWalletAsync(walletSettings);
        }

        /// <response code="204">A wallet settings successfully deleted.</response>
        [HttpDelete("wallets/{walletId}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public Task DeleteWalletAsync(string walletId)
        {
            return _walletSettingsService.DeleteWalletAsync(walletId);
        }

        /// <response code="204">An asset settings successfully deleted.</response>
        [HttpDelete("wallets/{walletId}/assets/{assetId}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public Task RemoveAssetFromWalletAsync(string walletId, string assetId)
        {
            return _walletSettingsService.RemoveAssetFromWalletAsync(walletId, assetId);
        }
    }
}
