using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain.AuditMessages;

namespace Lykke.Service.MarketMakerReports.Core.Repositories
{
    public interface IAuditMessageRepository
    {
        Task InsertAsync(AuditMessage auditMessage);

        Task<IReadOnlyList<AuditMessage>> GetAsync(DateTime? startDate, DateTime? endDate, string clientId = null);
        
        Task<IReadOnlyList<AuditMessage>> GetAllAsync();
    }
}
