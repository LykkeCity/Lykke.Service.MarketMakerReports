using System.Collections.Generic;
using System.Linq;
using Lykke.Service.MarketMakerReports.Core.Domain.PnL;

namespace Lykke.Service.MarketMakerReports.Services.PnL
{
    public class AssetRealisedPnLCache
    {
        private readonly object _sync = new object();
        
        private readonly Dictionary<string, Dictionary<string, AssetRealisedPnL>> _cache =
            new Dictionary<string, Dictionary<string, AssetRealisedPnL>>();

        public IReadOnlyCollection<AssetRealisedPnL> Get(string walletId)
        {
            lock (_sync)
            {
                if (_cache.ContainsKey(walletId))
                    return _cache[walletId].Values.ToArray();
            }

            return null;
        }

        public AssetRealisedPnL Get(string walletId, string assetId)
        {
            lock (_sync)
            {
                if (_cache.ContainsKey(walletId) && _cache[walletId].ContainsKey(assetId))
                    return _cache[walletId][assetId];
            }

            return null;
        }

        public void Set(AssetRealisedPnL assetRealisedPnL)
        {
            lock (_sync)
            {
                if(!_cache.ContainsKey(assetRealisedPnL.WalletId))
                    _cache[assetRealisedPnL.WalletId] = new Dictionary<string, AssetRealisedPnL>();
                
                _cache[assetRealisedPnL.WalletId][assetRealisedPnL.AssetId] = assetRealisedPnL;
            }
        }
        
        public void Initialize(AssetRealisedPnL assetRealisedPnL)
        {
            lock (_sync)
            {
                if (!_cache.ContainsKey(assetRealisedPnL.WalletId))
                {
                    _cache[assetRealisedPnL.WalletId] = new Dictionary<string, AssetRealisedPnL>
                    {
                        [assetRealisedPnL.AssetId] = assetRealisedPnL
                    };
                }
            }
        }
    }
}
