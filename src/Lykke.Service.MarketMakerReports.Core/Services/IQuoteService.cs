using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain;

namespace Lykke.Service.MarketMakerReports.Core.Services
{
    public interface IQuoteService
    {
        Task<Quote> GetAsync(string baseAssetId, string quoteAssetId);
    }
}
