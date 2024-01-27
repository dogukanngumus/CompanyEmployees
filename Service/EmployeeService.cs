using AutoMapper;
using Contracts;
using Entities;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.RequestFeatures;
using Shared.DataTransferObjects;
using System.Dynamic;

namespace Service;

public class EmployeeService(IRepositoryManager repository, IDataShaper<EmployeeDto> dataShaper, IMapper mapper) : IEmployeeService
{
    public async Task<EmployeeDto> GetCompanyEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
    {
       await CheckCompanyExists(companyId, trackChanges);
       var employee = await GetEmployeeIfExists(companyId, employeeId, trackChanges); 
       var employeeDtos = mapper.Map<EmployeeDto>(employee);
       return employeeDtos;
    }

    public async Task<(IEnumerable<ExpandoObject> employees, MetaData metaData)> GetCompanyEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters ,bool trackChanges)
    {
        if (!employeeParameters.ValidAgeRange)
        {
          throw new MaxAgeRangeBadRequestException();
        }
       
        await CheckCompanyExists(companyId, trackChanges);
        var employees = await repository.EmployeeRepository.GetEmployeesAsync(companyId, employeeParameters ,trackChanges);
        var employeeDtos = mapper.Map<IEnumerable<EmployeeDto>>(employees);
        var shapedData = dataShaper.GetShapedEntities(employeeDtos, employeeParameters.Fields);
        return (shapedData,employees.MetaData);
    }

    public  async Task<EmployeeDto> CreateEmployeeAsync(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool trackChanges)
    {
       await CheckCompanyExists(companyId, trackChanges);
       var employee = mapper.Map<Employee>(employeeForCreationDto);
       
       repository.EmployeeRepository.CreateEmployee(companyId, employee);
       await repository.SaveAsync();

       return mapper.Map<EmployeeDto>(employee);
    }

    public async Task DeleteEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
    {
       await CheckCompanyExists(companyId,trackChanges);
       var employee = await GetEmployeeIfExists(companyId,employeeId,trackChanges);

       repository.EmployeeRepository.DeleteEmployee(employee);
       await repository.SaveAsync();
    }

    public async Task UpdateEmployeeAsync(Guid companyId, Guid employeeId, EmployeeForUpdateDto employeeForUpdateDto, bool companyTrackChanges, bool employeeTrackChanges)
    {
        await CheckCompanyExists(companyId,companyTrackChanges);
        var employee = await GetCompanyEmployeeAsync(companyId,employeeId,employeeTrackChanges);
        mapper.Map(employeeForUpdateDto, employee);
        await repository.SaveAsync();
    }

    public async Task<(EmployeeForUpdateDto employeeForUpdate, Employee employee)> GetEmployeeForPatchAsync(Guid companyId, Guid employeeId, bool companyTrackChanges, bool employeeTrackChanges)
    {
       await CheckCompanyExists(companyId, companyTrackChanges);   
       var employee = await GetEmployeeIfExists(companyId,employeeId, employeeTrackChanges);
       var employeDto = mapper.Map<EmployeeForUpdateDto>(employee);
       return(employeDto, employee);
    }

    public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
    {
       mapper.Map(employeeToPatch, employeeEntity);
       await repository.SaveAsync();
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
