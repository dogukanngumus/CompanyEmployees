using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

public class ServiceManager(IRepositoryManager repository, IEmployeeLinks employeeLinks, IMapper mapper, ILoggerManager logger
,UserManager<User> userManager,IConfiguration configuration, IOptions<JwtConfiguration> options) : IServiceManager
{
    private readonly Lazy<IEmployeeService> _employeeService = new(()=> new EmployeeService(repository, employeeLinks, mapper));
    private readonly Lazy<ICompanyService> _companyService = new(()=> new CompanyService(repository,mapper));
    private readonly Lazy<IAuthenticationService> _authenticationService = new(()=> new AuthenticationService(logger, mapper,userManager,configuration,options));
    public IEmployeeService EmployeeService => _employeeService.Value;
    public ICompanyService CompanyService => _companyService.Value;

    public IAuthenticationService AuthenticationService => _authenticationService.Value;
}
