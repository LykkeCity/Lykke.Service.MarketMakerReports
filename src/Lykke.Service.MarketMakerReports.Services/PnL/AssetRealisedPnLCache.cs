using System.Collections.Generic;
using Lykke.Service.MarketMakerReports.Core.Domain.PnL;

namespace Lykke.Service.MarketMakerReports.Services.PnL
{
    public class AssetRealisedPnLCache
    {
        private readonly object _sync = new object();
        
        private readonly Dictionary<string, AssetRealisedPnL> _cache =
            new Dictionary<string, AssetRealisedPnL>();

        public AssetRealisedPnL Get(string assetId)
        {
            lock (_sync)
            {
                if (_cache.ContainsKey(assetId))
                    return _cache[assetId];
            }

            return null;
        }

        public void Set(AssetRealisedPnL assetRealisedPnL)
        {
            lock (_sync)
            {
                _cache[assetRealisedPnL.AssetId] = assetRealisedPnL;
            }
        }
    }
}
