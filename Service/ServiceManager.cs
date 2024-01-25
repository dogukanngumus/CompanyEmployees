using Contracts;
using Service.Contracts;

namespace Service;

public class ServiceManager(IRepositoryManager repository, ILoggerManager logger) : IServiceManager
{
    private readonly Lazy<IEmployeeService> _employeeService = new(()=> new EmployeeService(repository, logger));
    private readonly Lazy<ICompanyService> _companyService = new(()=> new CompanyService(repository, logger));
    public IEmployeeService EmployeeService => _employeeService.Value;
    public ICompanyService CompanyService => _companyService.Value;
}
