using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain.Settings;

namespace Lykke.Service.MarketMakerReports.Core.Repositories
{
    public interface IWalletSettingsRepository
    {
        Task<IReadOnlyCollection<WalletSettings>> GetAllAsync();
        
        Task<WalletSettings> GetByIdAsync(string walletId);

        Task InsertAsync(WalletSettings walletSettings);
        
        Task UpdateAsync(WalletSettings walletSettings);
        
        Task DeleteAsync(string walletId);
    }
}
