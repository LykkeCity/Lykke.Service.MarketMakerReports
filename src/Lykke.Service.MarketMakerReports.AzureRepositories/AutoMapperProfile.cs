using AutoMapper;
using JetBrains.Annotations;
using Lykke.Service.MarketMakerReports.Core.Domain.AuditMessages;

namespace Lykke.Service.MarketMakerReports.AzureRepositories
{
    [UsedImplicitly]
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AuditMessage, AuditMessageEntity>(MemberList.Source);
            CreateMap<AuditMessageEntity, AuditMessage>(MemberList.Destination);
        }
    }
}
