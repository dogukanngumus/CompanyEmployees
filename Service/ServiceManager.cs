using AutoMapper;
using Contracts;
using Service.Contracts;

namespace Service;

public class ServiceManager(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : IServiceManager
{
    private readonly Lazy<IEmployeeService> _employeeService = new(()=> new EmployeeService(repository, logger, mapper));
    private readonly Lazy<ICompanyService> _companyService = new(()=> new CompanyService(repository, logger,mapper));
    public IEmployeeService EmployeeService => _employeeService.Value;
    public ICompanyService CompanyService => _companyService.Value;
}
