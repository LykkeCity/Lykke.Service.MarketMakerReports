using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain.Settings;

namespace Lykke.Service.MarketMakerReports.Core.Repositories
{
    public interface IAssetRealisedPnLSettingsRepository
    {
        Task<AssetRealisedPnLSettings> GetAsync();

        Task InsertOrReplaceAsync(AssetRealisedPnLSettings assetRealisedPnLSettings);
    }
}
