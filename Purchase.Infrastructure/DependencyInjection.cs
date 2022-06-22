using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Purchase.Infrastructure.Services;
using Purchase.Infrastructure.Persistence.Interfaces;
using Purchase.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using EFCoreSecondLevelCacheInterceptor;
using Purchase.Infrastructure.Persistence.Interceptors;

namespace Purchase.Infrastructure
{
    public static class DependencyInjection
    {
        
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration _configuration)
        {
           
            services.AddSingleton(sp =>
            {
                string serviceAPIUrl = _configuration.GetValue<string>("APIURLs:ThriivePurchaseURL");
                ILogger<APIConnection> logger = sp.GetRequiredService<ILogger<APIConnection>>();
                return new APIConnection(logger,serviceAPIUrl);
            });
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());


            if (_configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("Demo"));
            }
            else
            {
                const string providerName1 = "Redis1";
                services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>

                    options
                    //.AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>())
                    .AddInterceptors(new WriteAuditInterceptor(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection")))
                    .UseSqlServer(
                        _configuration.GetValue<string>("ConnectionStrings:DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))



                    );
                services.AddEFSecondLevelCache(options =>
                {
                    
                    options.UseEasyCachingCoreProvider(providerName1, isHybridCache: false).DisableLogging(true).UseCacheKeyPrefix("EF_");
                });

                services.AddEasyCaching(option =>
                {
                    option.UseRedis(config =>
                    {

                        config.DBConfig.Endpoints.Add(new EasyCaching.Core.Configurations.ServerEndPoint(_configuration.GetValue<string>("CacheSettings:IPAddress"), _configuration.GetValue<int>("CacheSettings:Port")));
                    }, providerName1);
                });
            }


            


            return services;
        }
    }
}
