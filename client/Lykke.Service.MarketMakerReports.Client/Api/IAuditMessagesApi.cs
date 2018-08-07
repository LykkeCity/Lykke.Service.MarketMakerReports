using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Client.Models;
using Refit;

namespace Lykke.Service.MarketMakerReports.Client.Api
{
    public interface IAuditMessagesApi
    {
        [Get("/api/auditmessages")]
        Task<IReadOnlyList<AuditMessageModel>> Get(DateTime? from, DateTime? to, string clientId = null);
    }
}
