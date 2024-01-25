﻿using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class EmployeeRepository(RepositoryContext context) : RepositoryBase<Employee>(context), IEmployeeRepository
{
    public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
    => await FindByCondition(e=> e.CompanyId.Equals(companyId) && e.Id.Equals(employeeId), trackChanges).FirstOrDefaultAsync();

    public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, bool trackChanges)
    => await FindByCondition(e=> e.CompanyId.Equals(companyId), trackChanges).OrderBy(e=> e.Name).ToListAsync();
}
