using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.MarketMakerReports.Client.Models.Settings;
using Refit;

namespace Lykke.Service.MarketMakerReports.Client.Api
{
    [PublicAPI]
    public interface ISettingsApi
    {
        [Get("/api/settings/realisedpnl")]
        Task<AssetRealisedPnLSettingsModel> GetRealisedPnLSettingsAsync();

        [Post("/api/settings/realisedpnl")]
        Task UpdateRealisedPnLSettingsAsync(AssetRealisedPnLSettingsModel model);
    }
}
