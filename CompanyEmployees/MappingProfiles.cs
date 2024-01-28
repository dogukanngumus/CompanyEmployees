using AutoMapper;
using Entities.Models;
using Shared;
using Shared.DataTransferObjects;

namespace CompanyEmployees;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Company, CompanyDto>().ForMember(c=> c.FullAddress, c=> c.MapFrom(x=> String.Join(x.Address," ",x.Country)));
        CreateMap<Employee, EmployeeDto>();

        CreateMap<CompanyForCreationDto, Company>();
        CreateMap<EmployeeForCreationDto, Employee>();

        CreateMap<CompanyForUpdateDto, Company>().ReverseMap();
        CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();

        CreateMap<UserForRegistrationDto, User>();
    }
}
