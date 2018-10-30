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
                    Trading = ExchangePnLs.Sum(x => x.Trading),
                    Directional = ExchangePnLs.Sum(x => x.Directional)
                };
            }
        }
    }
}
