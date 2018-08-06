using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.MarketMakerReports.Core.Domain.AuditMessages;
using Lykke.Service.MarketMakerReports.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.MarketMakerReports.Controllers
{
    [Route("/api/[controller]")]
    public class AuditMessageController : Controller
    {
        private readonly IAuditMessageService _auditMessageService;

        public AuditMessageController(IAuditMessageService auditMessageService)
        {
            _auditMessageService = auditMessageService;
        }
        
        /// <response code="200">Audit messages</response>
        [HttpGet]
        [SwaggerOperation("AuditMessageGetAll")]
        [ProducesResponseType(typeof(AuditMessage), (int) HttpStatusCode.OK)]
        public async Task<IReadOnlyList<AuditMessage>> GetAll()
        {
            return await _auditMessageService.GetAllAsync();
        }
    }
}
