﻿using AutoMapper;
using JetBrains.Annotations;
using Lykke.Service.MarketMakerReports.Core.Domain.AuditMessages;
using Lykke.Service.MarketMakerReports.Core.Domain.PnL;
using Lykke.Service.MarketMakerReports.Core.Domain.Settings;
using Lykke.Service.MarketMakerReports.Core.Domain.Trades;

namespace Lykke.Service.MarketMakerReports.AzureRepositories
{
    [UsedImplicitly]
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AuditMessage, AuditMessageEntity>(MemberList.Source);
            CreateMap<AuditMessageEntity, AuditMessage>(MemberList.Destination);

            CreateMap<LykkeTrade, LykkeTradeEntity>(MemberList.Source)
                .ForSourceMember(src => src.Exchange, opt => opt.Ignore());
            CreateMap<LykkeTradeEntity, LykkeTrade>(MemberList.Destination)
                .ForMember(dest => dest.Exchange, opt => opt.Ignore());
            
            CreateMap<ExternalTrade, ExternalTradeEntity>(MemberList.Source)
                .ForSourceMember(src => src.Id, opt => opt.Ignore());
            CreateMap<ExternalTradeEntity, ExternalTrade>(MemberList.Destination)
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            
            CreateMap<AssetRealisedPnL, AssetRealisedPnLEntity>(MemberList.Source);
            CreateMap<AssetRealisedPnLEntity, AssetRealisedPnL>(MemberList.Destination);
            
            CreateMap<AssetRealisedPnLSettings, AssetRealisedPnLSettingsEntity>(MemberList.Source);
            CreateMap<AssetRealisedPnLSettingsEntity, AssetRealisedPnLSettings>(MemberList.Destination);
        }
    }
}
