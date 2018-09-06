using System.Collections.Generic;
using Lykke.Service.MarketMakerReports.Core.Domain.Settings;

namespace Lykke.Service.MarketMakerReports.Services.Settings
{
    public class RealisedPnLSettingsCache
    {
        private readonly object _sync = new object();

        private readonly Dictionary<string, WalletSettings> _wallets = new Dictionary<string, WalletSettings>();

        public IReadOnlyCollection<WalletSettings> GetWallets()
        {
            lock (_sync)
            {
                return _wallets.Values;
            }
        }

        public WalletSettings GetWallet(string walletId)
        {
            lock (_sync)
            {
                if (_wallets.ContainsKey(walletId))
                    return _wallets[walletId];
            }

            return null;
        }

        public void Set(WalletSettings walletSettings)
        {
            lock (_sync)
            {
                _wallets[walletSettings.Id] = walletSettings;
            }
        }

        public void Remove(string walletId)
        {
            lock (_sync)
            {
                if (_wallets.ContainsKey(walletId))
                    _wallets.Remove(walletId);
            }
        }
    }
}
