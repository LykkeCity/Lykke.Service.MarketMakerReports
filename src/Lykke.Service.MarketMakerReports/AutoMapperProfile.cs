using AutoMapper;
using Lykke.Service.MarketMakerReports.Client.Models.AuditMessages;
using Lykke.Service.MarketMakerReports.Client.Models.InventorySnapshots;
using Lykke.Service.MarketMakerReports.Client.Models.PnL;
using Lykke.Service.MarketMakerReports.Core.Domain.PnL;
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

            CreateMap<InventorySnapshot, Core.Domain.InventorySnapshots.InventorySnapshot>(MemberList.None);

            CreateMap<Core.Domain.InventorySnapshots.InventorySnapshot, InventorySnapshotModel>(MemberList.Destination);

            CreateMap<PnLResult, PnLResultModel>(MemberList.Destination);
        }
    }
}
