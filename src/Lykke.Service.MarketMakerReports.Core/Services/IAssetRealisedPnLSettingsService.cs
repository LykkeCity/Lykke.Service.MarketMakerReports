using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain.Settings;

namespace Lykke.Service.MarketMakerReports.Core.Services
{
    public interface IAssetRealisedPnLSettingsService
    {
        Task<AssetRealisedPnLSettings> GetAsync();

        Task SaveAsync(AssetRealisedPnLSettings assetRealisedPnLSettings);
    }
}
