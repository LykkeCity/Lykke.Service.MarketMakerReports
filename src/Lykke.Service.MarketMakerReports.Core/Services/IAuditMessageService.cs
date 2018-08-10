using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain.AuditMessages;

namespace Lykke.Service.MarketMakerReports.Core.Services
{
    public interface IAuditMessageService
    {
        Task HandleAsync(AuditMessage auditMessage);
        
        Task<IReadOnlyList<AuditMessage>> GetAllAsync();
        
        Task<IReadOnlyList<AuditMessage>> GetAsync(DateTime? startDate, DateTime? endDate, string clientId);
    }
}
