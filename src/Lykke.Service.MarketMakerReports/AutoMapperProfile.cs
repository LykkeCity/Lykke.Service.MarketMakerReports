using System.Collections.Generic;
using AutoMapper;
using Lykke.Service.MarketMakerReports.Client.Models.AuditMessages;
using Lykke.Service.MarketMakerReports.Client.Models.Health;
using Lykke.Service.MarketMakerReports.Client.Models.InventorySnapshots;
using Lykke.Service.MarketMakerReports.Client.Models.PnL;
using Lykke.Service.MarketMakerReports.Core.Domain.Health;
using Lykke.Service.MarketMakerReports.Client.Models.RealisedPnLSettings;
using Lykke.Service.MarketMakerReports.Client.Models.Trades;
using Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots;
using Lykke.Service.MarketMakerReports.Core.Domain.PnL;
using Lykke.Service.MarketMakerReports.Core.Domain.Settings;
using Lykke.Service.MarketMakerReports.Core.Domain.Trades;
using Lykke.Service.NettingEngine.Contract.Audit;
using HealthIssueContract = Lykke.Service.MarketMakerReports.Contracts.HealthIssues.HealthIssue;

namespace Lykke.Service.MarketMakerReports
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AuditEvent, Core.Domain.AuditMessages.AuditMessage>(MemberList.Source);

            CreateMap<Core.Domain.AuditMessages.AuditMessage, AuditMessageModel>(MemberList.Destination);

            CreateMap<InventorySnapshot, InventorySnapshot>(MemberList.None);

            CreateMap<AssetBalanceInventory, AssetBalanceInventory>()
                .ForMember(x => x.Balances, m => m.MapFrom(x => x.Balances ?? new List<AssetBalance>()))
                .ForMember(x => x.Inventories, m => m.MapFrom(x => x.Inventories ?? new List<AssetInventory>()));

            CreateMap<InventorySnapshot, InventorySnapshotModel>(MemberList.Destination);

            CreateMap<InventorySnapshot, InventorySnapshotBriefModel>(MemberList.Destination);

            CreateMap<AssetBalanceInventory, AssetBalanceInventoryModel>();


            CreateMap<HealthIssue, HealthIssueModel>(MemberList.Destination);
            CreateMap<HealthIssueContract, HealthIssue>(MemberList.Source);

            CreateMap<LykkeTrade, LykkeTradeModel>(MemberList.Source);
            CreateMap<ExternalTrade, ExternalTradeModel>(MemberList.Source);

            CreateMap<AssetRealisedPnL, AssetRealisedPnLModel>(MemberList.Source);

            CreateMap<WalletSettings, WalletSettingsModel>(MemberList.Source);
            CreateMap<WalletSettingsModel, WalletSettings>(MemberList.Destination)
                .ForMember(dest => dest.Assets, opt => opt.Ignore());

            CreateMap<NettingEngine.Contract.Trades.LykkeTrade, LykkeTrade>(MemberList.Source);
            CreateMap<NettingEngine.Contract.Trades.ExternalTrade, ExternalTrade>(MemberList.Source);

            // P&L

            CreateMap<PnLResult, PnLResultModel>(MemberList.Source);

            CreateMap<ExchangePnL, ExchangePnLModel>(MemberList.Source);

            CreateMap<AssetPnL, AssetPnLModel>(MemberList.Source)
                .ForSourceMember(src => src.Exchange, opt => opt.Ignore())
                .ForMember(src => src.StartBalance, opt => opt.MapFrom(o => o.StartBalance.Balance))
                .ForMember(src => src.StartBalanceInUsd, opt => opt.MapFrom(o => o.StartBalance.BalanceInUsd))
                .ForMember(src => src.StartPrice, opt => opt.MapFrom(o => o.StartBalance.Price))
                .ForMember(src => src.EndBalance, opt => opt.MapFrom(o => o.EndBalance.Balance))
                .ForMember(src => src.EndBalanceInUsd, opt => opt.MapFrom(o => o.EndBalance.BalanceInUsd))
                .ForMember(src => src.EndPrice, opt => opt.MapFrom(o => o.EndBalance.Price));
        }
    }
}
