using AutoMapper;
using Customer.Data.Models;
using Customer.Model.Environment;

namespace Customer.Model.Mappings
{
    public class TenantProfile : Profile
    {
        public TenantProfile()
        {
            CreateMap<EnvironmentDto, TenantEnvironment>()
                .ReverseMap();
          
        }
    }
}
