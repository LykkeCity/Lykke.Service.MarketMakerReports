using System.Collections.Generic;
using System.Linq;

namespace Lykke.Service.MarketMakerReports.Core.Domain.Settings
{
    public class WalletSettings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        public bool HandleExternalTrades { get; set; }

        public IReadOnlyCollection<string> Assets { get; set; }

        public void AddAsset(string assetId)
        {
            Assets = (Assets ?? new string[0])
                .Union(new[] {assetId})
                .ToArray();
        }

        public void RemoveAsset(string assetId)
        {
            Assets = (Assets ?? new string[0])
                .Except(new[] {assetId})
                .ToArray();
        }

        public void Update(WalletSettings walletSettings)
        {
            Name = walletSettings.Name;
            Enabled = walletSettings.Enabled;
            HandleExternalTrades = walletSettings.HandleExternalTrades;
        }
    }
}
