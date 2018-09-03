using Lykke.Service.MarketMakerReports.Core.Domain;
using Lykke.Service.MarketMakerReports.Core.Domain.PnL;
using Lykke.Service.MarketMakerReports.Core.Domain.Trades;

namespace Lykke.Service.MarketMakerReports.Core.Math
{
    public interface IAssetRealisedPnLCalculator
    {
        AssetRealisedPnL Calculate(AssetRealisedPnL previousPnL, Trade trade, Quote quote, Quote crossQuote, string assetId);
    }
}
