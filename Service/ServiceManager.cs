using AutoMapper;
using Contracts;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

public class ServiceManager(IRepositoryManager repository, IEmployeeLinks employeeLinks, IMapper mapper) : IServiceManager
{
    private readonly Lazy<IEmployeeService> _employeeService = new(()=> new EmployeeService(repository, employeeLinks, mapper));
    private readonly Lazy<ICompanyService> _companyService = new(()=> new CompanyService(repository,mapper));
    public IEmployeeService EmployeeService => _employeeService.Value;
    public ICompanyService CompanyService => _companyService.Value;
}
