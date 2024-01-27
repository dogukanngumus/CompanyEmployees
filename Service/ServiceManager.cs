using AutoMapper;
using Contracts;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

public class ServiceManager(IRepositoryManager repository, IDataShaper<EmployeeDto> dataShaper, IMapper mapper) : IServiceManager
{
    private readonly Lazy<IEmployeeService> _employeeService = new(()=> new EmployeeService(repository, dataShaper, mapper));
    private readonly Lazy<ICompanyService> _companyService = new(()=> new CompanyService(repository,mapper));
    public IEmployeeService EmployeeService => _employeeService.Value;
    public ICompanyService CompanyService => _companyService.Value;
}
