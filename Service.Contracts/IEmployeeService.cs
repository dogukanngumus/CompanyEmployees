using Entities.Models;
using Shared.RequestFeatures;
using Shared.DataTransferObjects;
using System.Dynamic;

namespace Service.Contracts;

public interface IEmployeeService
{
    Task<(IEnumerable<ExpandoObject> employees, MetaData metaData)> GetCompanyEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters ,bool trackChanges);
    Task<EmployeeDto> GetCompanyEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges);
    Task<EmployeeDto> CreateEmployeeAsync(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool trackChanges);
    Task DeleteEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges);
    Task UpdateEmployeeAsync(Guid companyId, Guid employeeId, EmployeeForUpdateDto employeeForUpdateDto, bool companyTrackChanges, bool employeeTrackChanges);
    Task<(EmployeeForUpdateDto employeeForUpdate, Employee employee)> GetEmployeeForPatchAsync(Guid companyId, Guid employeeId, bool companyTrackChanges, bool employeeTrackChanges);
    Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity);

}
