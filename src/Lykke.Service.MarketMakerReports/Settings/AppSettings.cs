﻿using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using Lykke.Service.MarketMakerReports.Settings.ServiceSettings;

namespace Lykke.Service.MarketMakerReports.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public MarketMakerReportsSettings MarketMakerReportsService { get; set; }
    }
}