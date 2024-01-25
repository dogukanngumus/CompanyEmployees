using AutoMapper;
using Contracts;
using Entities;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared;

namespace Service;

public class EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : IEmployeeService
{
    public async Task<EmployeeDto> GetCompanyEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
    {
       await CheckCompanyExists(companyId, trackChanges);
       var employee = await GetEmployeeIfExists(companyId, employeeId, trackChanges); 
       var employeeDtos = mapper.Map<EmployeeDto>(employee);
       return employeeDtos;
    }

    public async Task<IEnumerable<EmployeeDto>> GetCompanyEmployeesAsync(Guid companyId, bool trackChanges)
    {
        await CheckCompanyExists(companyId, trackChanges);
        var employees = await repository.EmployeeRepository.GetEmployeesAsync(companyId, trackChanges);
        return mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }

    private async Task CheckCompanyExists(Guid companyId, bool trackChanges)
    {
       var company = await repository.CompanyRepository.GetCompanyAsync(companyId, trackChanges);
       if(company is null)
       {
         throw new CompanyNotFoundException(companyId);
       }
    }

    private async Task<Employee> GetEmployeeIfExists(Guid companyId, Guid employeeId, bool trackChanges)
    {
       var employee = await repository.EmployeeRepository.GetEmployeeAsync(companyId,employeeId,trackChanges);
       if(employee is null)
       {
          throw new EmployeeNotFoundException(employeeId);
       }
       return employee;
    }
}
