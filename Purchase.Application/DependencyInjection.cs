using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Purchase.Application.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Purchase.Application.EventHandlers;
using Purchase.Core.Events;
using Microsoft.Extensions.Configuration;
using Purchase.Application.ApplicationLogic;
using Purchase.Application.Repositories.Interfaces;
using Purchase.Application.Repositories;
using Purchase.Application.Mappings;

namespace Purchase.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
                this IServiceCollection services,
                IConfiguration configuration

            )
        {

            
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });


            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            
            services.AddTransient<IAirtimePurchaseRepository, AirtimePurchaseRepository>();

            services.AddTransient<AirtimePurchaseApplicationLogic>();
            
            return services;
        }
    }
}
