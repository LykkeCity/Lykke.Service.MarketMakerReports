using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.MarketMakerReports.Client.Models.AuditMessages;
using Refit;

namespace Lykke.Service.MarketMakerReports.Client.Api
{
    [PublicAPI]
    public interface IAuditMessagesApi
    {
        [Get("/api/auditmessages")]
        Task<IReadOnlyList<AuditMessageModel>> Get(DateTime? from, DateTime? to, string clientId = null);
    }
}
