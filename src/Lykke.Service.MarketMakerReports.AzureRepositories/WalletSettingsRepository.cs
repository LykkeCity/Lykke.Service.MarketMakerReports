using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.MarketMakerReports.Core.Domain.Settings;
using Lykke.Service.MarketMakerReports.Core.Repositories;

namespace Lykke.Service.MarketMakerReports.AzureRepositories
{
    public class WalletSettingsRepository : IWalletSettingsRepository
    {
        private readonly INoSQLTableStorage<WalletSettingsEntity> _storage;

        public WalletSettingsRepository(INoSQLTableStorage<WalletSettingsEntity> storage)
        {
            _storage = storage;
        }

        public async Task<IReadOnlyCollection<WalletSettings>> GetAllAsync()
        {
            IEnumerable<WalletSettingsEntity> entities = await _storage.GetDataAsync(GetPartitionKey());

            return Mapper.Map<List<WalletSettings>>(entities);
        }

        public async Task<WalletSettings> GetByIdAsync(string walletId)
        {
            WalletSettingsEntity entity = await _storage.GetDataAsync(GetPartitionKey(), GetRowKey(walletId));

            return Mapper.Map<WalletSettings>(entity);
        }

        public async Task InsertAsync(WalletSettings walletSettings)
        {
            var entity = new WalletSettingsEntity(GetPartitionKey(), GetRowKey(walletSettings.Id));

            Mapper.Map(walletSettings, entity);

            await _storage.InsertAsync(entity);
        }

        public async Task UpdateAsync(WalletSettings walletSettings)
        {
            var entity = new WalletSettingsEntity(GetPartitionKey(), GetRowKey(walletSettings.Id));

            Mapper.Map(walletSettings, entity);

            await _storage.ReplaceAsync(entity);
        }

        public Task DeleteAsync(string walletId)
        {
            return _storage.DeleteAsync(GetPartitionKey(), GetRowKey(walletId));
        }

        private static string GetPartitionKey()
            => "wallet";

        private static string GetRowKey(string walletId)
            => walletId.ToLower();
    }
}
