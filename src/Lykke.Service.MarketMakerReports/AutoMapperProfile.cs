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

            CreateMap<Core.Domain.InventorySnapshots.AssetBalanceInventory, AssetBalanceInventoryModel>()
                .ForMember(x => x.Asset, m => m.ResolveUsing(x => x.AssetDisplayId ?? x.AssetId));

            CreateMap<PnLResult, PnLResultModel>(MemberList.Destination);

            CreateMap<AssetPnL, AssetPnLModel>()
                .ForMember(x => x.StartBalance, m => m.MapFrom(x => x.StartBalance.Balance))
                .ForMember(x => x.StartBalanceInUsd, m => m.MapFrom(x => x.StartBalance.BalanceInUsd))
                .ForMember(x => x.StartPrice, m => m.MapFrom(x => x.StartBalance.Price))
                .ForMember(x => x.EndBalance, m => m.MapFrom(x => x.EndBalance.Balance))
                .ForMember(x => x.EndBalanceInUsd, m => m.MapFrom(x => x.EndBalance.BalanceInUsd))
                .ForMember(x => x.EndPrice, m => m.MapFrom(x => x.EndBalance.Price));
        }
    }
}
