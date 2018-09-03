using System.Collections.Generic;

namespace Lykke.Service.MarketMakerReports.Client.Models.Trades
{
    public class ContinuationTokenResult<T>
    {
        public IEnumerable<T> Entities { get; set; }
        
        public string ContinuationToken { get; set; }
    }
}
