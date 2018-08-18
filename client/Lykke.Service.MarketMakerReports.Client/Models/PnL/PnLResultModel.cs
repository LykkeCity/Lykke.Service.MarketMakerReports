using System;
using System.Collections.Generic;

namespace Lykke.Service.MarketMakerReports.Client.Models.PnL
{
    public class PnLResultModel
    {
        public decimal Adjusted { get; set; }
        
        public decimal Directional { get; set; }

        public decimal Total { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public IReadOnlyList<AssetPnLModel> AssetsPnLs { get; set; }
    }
}
