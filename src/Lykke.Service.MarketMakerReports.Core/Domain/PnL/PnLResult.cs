using System;
using System.Collections.Generic;

namespace Lykke.Service.MarketMakerReports.Core.Domain.PnL
{
    public class PnLResult
    {
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public decimal Adjusted { get; set; }
        
        public decimal Directional { get; set; }

        public decimal Total => Adjusted + Directional;
        
        public IReadOnlyList<AssetPnL> AssetsPnLs { get; set; }
    }
}
