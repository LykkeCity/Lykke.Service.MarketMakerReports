using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.MarketMakerReports.Core.Domain.AuditMessages;
using Lykke.Service.MarketMakerReports.Core.Repositories;
using Lykke.Service.MarketMakerReports.Core.Services;

namespace Lykke.Service.MarketMakerReports.Services
{
    public class AuditMessageService : IAuditMessageService
    {
        private readonly IAuditMessageRepository _auditMessageRepository;
        private readonly ILog _log;

        public AuditMessageService(ILogFactory logFactory,
            IAuditMessageRepository auditMessageRepository)
        {
            _auditMessageRepository = auditMessageRepository;
            _log = logFactory.CreateLog(this);
        }
        
        public async Task HandleAsync(AuditMessage auditMessage)
        {
            if (auditMessage.CreationDate == default)
            {
                auditMessage.CreationDate = DateTime.UtcNow;
            }

            await _auditMessageRepository.InsertAsync(auditMessage);
        }

        public Task<IReadOnlyList<AuditMessage>> GetAllAsync()
        {
            return _auditMessageRepository.GetAllAsync();
        }
    }
}
