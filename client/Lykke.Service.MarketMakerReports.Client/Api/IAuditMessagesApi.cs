using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Client.Models;

namespace Lykke.Service.MarketMakerReports.Client.Api
{
    public interface IAuditMessagesApi
    {
        Task<IReadOnlyList<AuditMessageModel>> Get(DateTime? from, DateTime? to, string clientId = null);
    }
}
