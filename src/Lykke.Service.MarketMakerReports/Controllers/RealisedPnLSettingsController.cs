using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Client.Api;
using Lykke.Service.MarketMakerReports.Client.Models.RealisedPnLSettings;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.MarketMakerReports.Controllers
{
    [Route("/api/[controller]")]
    public class RealisedPnLSettingsController : Controller, IRealisedPnLSettingsApi
    {
        /// <response code="200">A collection of settings of wallets.</response>
        [HttpGet("wallets")]
        [ProducesResponseType(typeof(IReadOnlyCollection<WalletSettingsModel>), (int)HttpStatusCode.OK)]
        public Task<IReadOnlyCollection<WalletSettingsModel>> GetWalletsAsync()
        {
            throw new System.NotImplementedException();
        }

        /// <response code="200">A wallet settings.</response>
        [HttpGet("wallets/{walletId}")]
        [ProducesResponseType(typeof(IReadOnlyCollection<WalletSettingsModel>), (int)HttpStatusCode.OK)]
        public Task<WalletSettingsModel> GetWalletAsync(string walletId)
        {
            throw new System.NotImplementedException();
        }

        /// <response code="200">A collection of settings of assets.</response>
        [HttpGet("wallets/{walletId}/assets")]
        [ProducesResponseType(typeof(IReadOnlyCollection<WalletSettingsModel>), (int)HttpStatusCode.OK)]
        public Task<IReadOnlyCollection<AssetSettingsModel>> GetAssetsAsync(string walletId)
        {
            throw new System.NotImplementedException();
        }

        /// <response code="204">A wallet settings successfully added.</response>
        [HttpPost("wallets")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public Task AddWalletAsync(WalletSettingsModel walletSettingsModel)
        {
            throw new System.NotImplementedException();
        }

        /// <response code="204">An asset settings successfully added.</response>
        [HttpPost("assets")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public Task AddAssetAsync(AssetSettingsModel assetSettingsModel)
        {
            throw new System.NotImplementedException();
        }

        /// <response code="204">A wallet settings successfully updated.</response>
        [HttpPut("wallets")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public Task UpdateWalletAsync(WalletSettingsModel walletSettingsModel)
        {
            throw new System.NotImplementedException();
        }

        /// <response code="204">A wallet settings successfully deleted.</response>
        [HttpDelete("wallets/{walletId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public Task DeleteWalletAsync(string walletId)
        {
            throw new System.NotImplementedException();
        }

        /// <response code="204">An asset settings successfully deleted.</response>
        [HttpDelete("wallets/{walletId}/assets/{assetId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public Task DeleteAssetAsync(string walletId, string assetId)
        {
            throw new System.NotImplementedException();
        }
    }
}
