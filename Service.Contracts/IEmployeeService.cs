using Entities.Models;
using Shared.RequestFeatures;
using Shared.DataTransferObjects;
using System.Dynamic;
using Entities;

namespace Service.Contracts;

public interface IEmployeeService
{
    Task<(LinkResponse linkResponse, MetaData metaData)> GetCompanyEmployeesAsync(Guid companyId, LinkParameters linkParameters ,bool trackChanges);
    Task<EmployeeDto> GetCompanyEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges);
    Task<EmployeeDto> CreateEmployeeAsync(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool trackChanges);
    Task DeleteEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges);
    Task UpdateEmployeeAsync(Guid companyId, Guid employeeId, EmployeeForUpdateDto employeeForUpdateDto, bool companyTrackChanges, bool employeeTrackChanges);
    Task<(EmployeeForUpdateDto employeeForUpdate, Employee employee)> GetEmployeeForPatchAsync(Guid companyId, Guid employeeId, bool companyTrackChanges, bool employeeTrackChanges);
    Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity);

}
