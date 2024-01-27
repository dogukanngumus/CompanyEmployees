using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;

namespace Repositories;

public class EmployeeRepository(RepositoryContext context) : RepositoryBase<Employee>(context), IEmployeeRepository
{
    public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
    => await FindByCondition(e=> e.CompanyId.Equals(companyId) && e.Id.Equals(employeeId), trackChanges).FirstOrDefaultAsync();

    public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters,bool trackChanges)
    {
        var employees = await FindByCondition(e=> e.CompanyId.Equals(companyId), trackChanges)
        .FilterEmployees(employeeParameters.MinAge, employeeParameters.MaxAge)
        .Search(employeeParameters.SearchTerm)
        .Sort(employeeParameters.OrderBy).ToListAsync();
        return PagedList<Employee>.ToPagedList(employees,employeeParameters.PageNumber, employeeParameters.PageSize);
    } 

    public void CreateEmployee(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;
        Create(employee);
    } 

    public void DeleteEmployee(Employee employee)
    => Delete(employee);
}
