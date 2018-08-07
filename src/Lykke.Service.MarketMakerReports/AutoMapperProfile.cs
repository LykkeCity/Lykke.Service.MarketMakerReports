using AutoMapper;
using Lykke.Service.MarketMakerReports.Client.Models;
using Lykke.Service.MarketMakerReports.Client.Models.AuditMessages;
using Lykke.Service.MarketMakerReports.Client.Models.InventorySnapshots;
using Lykke.Service.NettingEngine.Client.RabbitMq;
using Lykke.Service.NettingEngine.Client.RabbitMq.InventorySnapshots;

namespace Lykke.Service.MarketMakerReports
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AuditMessage, Core.Domain.AuditMessages.AuditMessage>(MemberList.Source);

            CreateMap<Core.Domain.AuditMessages.AuditMessage, AuditMessageModel>(MemberList.Destination);

            CreateMap<InventorySnapshot, Core.Domain.InventorySnapshots.InventorySnapshot>(MemberList.Source);

            CreateMap<Core.Domain.InventorySnapshots.InventorySnapshot, InventorySnapshotModel>(MemberList.Destination);
        }
    }
}
