using AutoMapper;
using Customer.Model.Company;

namespace Customer.Model.Mappings
{
    public class CompanyProfile: Profile
    {
        public CompanyProfile()
        {
            CreateMap<CompanyDto, Data.Models.Company>().ReverseMap();
        }
    }
}
