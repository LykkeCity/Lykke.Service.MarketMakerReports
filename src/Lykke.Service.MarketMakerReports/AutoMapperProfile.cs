using AutoMapper;
using Lykke.Service.MarketMakerReports.Client.Models;
using Lykke.Service.NettingEngine.Client.RabbitMq;

namespace Lykke.Service.MarketMakerReports
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AuditMessage, Core.Domain.AuditMessages.AuditMessage>(MemberList.Source);

            CreateMap<Core.Domain.AuditMessages.AuditMessage, AuditMessageModel>(MemberList.Destination);
        }
    }
}
