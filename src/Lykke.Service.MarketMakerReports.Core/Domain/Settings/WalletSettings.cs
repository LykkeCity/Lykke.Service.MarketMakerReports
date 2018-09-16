using System;
using System.Collections.Generic;
using System.Linq;

namespace Lykke.Service.MarketMakerReports.Core.Domain.Settings
{
    public class WalletSettings
    {
        private IReadOnlyCollection<string> _assets;

        public string Id { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        public bool HandleExternalTrades { get; set; }

        public IReadOnlyCollection<string> Assets
        {
            get => _assets ?? new string[0];
            set => _assets = value;
        }

        public void AddAsset(string assetId)
        {
            Assets = Assets
                .Union(new[] {assetId})
                .ToArray();
        }

        public void RemoveAsset(string assetId)
        {
            Assets = Assets
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
