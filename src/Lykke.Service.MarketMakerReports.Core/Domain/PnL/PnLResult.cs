using System;
using System.Collections.Generic;
using System.Linq;

namespace Lykke.Service.MarketMakerReports.Core.Domain.PnL
{
    public class PnLResult
    {
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public IEnumerable<ExchangePnL> ExchangePnLs { get; set; }

        public ExchangePnL OnAllExchanges
        {
            get
            {
                return new ExchangePnL
                {
                    Exchange = "Total",
                    Adjusted = ExchangePnLs.Sum(x => x.Adjusted),
                    Directional = ExchangePnLs.Sum(x => x.Directional),
                    AssetsPnLs = ExchangePnLs.SelectMany(x => x.AssetsPnLs).ToList()
                };
            }
        }
    }
}
