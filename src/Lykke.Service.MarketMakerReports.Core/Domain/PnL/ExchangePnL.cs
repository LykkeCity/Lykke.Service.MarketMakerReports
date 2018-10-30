using System.Collections.Generic;

namespace Lykke.Service.MarketMakerReports.Core.Domain.PnL
{
    public class ExchangePnL
    {
        public string Exchange { get; set; }

        public decimal Trading { get; set; }

        public decimal Directional { get; set; }

        public decimal Total => Trading + Directional;

        public IReadOnlyList<AssetPnL> AssetsPnLs { get; set; }
    }
}
