using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Purchase.Application.DTO.Purchase;
using Purchase.Core.Entities;

namespace Purchase.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());

            CreateMap<MobileTransactionDTO, MobileTransaction>()

                .ForPath(x => x.Id, c => c.MapFrom(y => y.EntertainmentTransactionId));
                
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i => 
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod("Mapping") 
                                 ?? type.GetInterface("IMapFrom`1").GetMethod("Mapping");
                
                methodInfo?.Invoke(instance, new object[] { this });

            }
        }
    }
}