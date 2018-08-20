using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.MarketMakerReports.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using AutoMapper;
using Lykke.Common.Api.Contract.Responses;

namespace Lykke.Service.MarketMakerReports
{
    [UsedImplicitly]
    public class Startup
    {
        private readonly LykkeSwaggerOptions _swaggerOptions = new LykkeSwaggerOptions
        {
            ApiTitle = "MarketMakerReports API",
            ApiVersion = "v1"
        };

        [UsedImplicitly]
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.BuildServiceProvider<AppSettings>(options =>
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.AddProfiles(typeof(AzureRepositories.AutoMapperProfile));
                    cfg.AddProfiles(typeof(AutoMapperProfile));
                });
                
                Mapper.AssertConfigurationIsValid();
                
                options.SwaggerOptions = _swaggerOptions;

                options.Logs = logs =>
                {
                    logs.AzureTableName = "MarketMakerReportsLog";
                    logs.AzureTableConnectionStringResolver = settings
                        => settings.MarketMakerReportsService.Db.LogsConnectionString;
                };

                options.Swagger = swagger => { swagger.IgnoreObsoleteActions(); };
            });
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app)
        {
            app.UseLykkeConfiguration(options =>
            {
                options.SwaggerOptions = _swaggerOptions;
                options.DefaultErrorHandler = exception => ErrorResponse.Create(exception.Message);
            });
        }
    }
}
