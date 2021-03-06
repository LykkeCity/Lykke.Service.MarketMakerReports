﻿using AutoMapper;
using JetBrains.Annotations;
using Lykke.Service.MarketMakerReports.Core.Domain.AuditMessages;
using Lykke.Service.MarketMakerReports.Core.Domain.PnL;
using Lykke.Service.MarketMakerReports.Core.Domain.Settings;
using Lykke.Service.MarketMakerReports.Core.Domain.Health;
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

            CreateMap<LykkeTrade, LykkeTradeEntity>(MemberList.Source);
            CreateMap<LykkeTradeEntity, LykkeTrade>(MemberList.Destination);
            
            CreateMap<ExternalTrade, ExternalTradeEntity>(MemberList.Source);
            CreateMap<ExternalTradeEntity, ExternalTrade>(MemberList.Destination);

            CreateMap<HealthIssue, HealthIssueEntity>(MemberList.Source);
            CreateMap<HealthIssueEntity, HealthIssue>(MemberList.Destination);
            
            CreateMap<AssetRealisedPnL, AssetRealisedPnLEntity>(MemberList.Source);
            CreateMap<AssetRealisedPnLEntity, AssetRealisedPnL>(MemberList.Destination);
            
            CreateMap<WalletSettings, WalletSettingsEntity>(MemberList.Source);
            CreateMap<WalletSettingsEntity, WalletSettings>(MemberList.Destination);
        }
    }
}
