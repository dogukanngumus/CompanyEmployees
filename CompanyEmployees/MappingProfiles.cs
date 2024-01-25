using AutoMapper;
using Entities.Models;
using Shared;

namespace CompanyEmployees;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Company, CompanyDto>().ForMember(c=> c.FullAddress, c=> c.MapFrom(x=> String.Join(x.Address," ",x.Country)));
         CreateMap<Employee, EmployeeDto>();
    }
}
