using Contracts;
using Entities.Models;

namespace Repositories;

public class EmployeeRepository(RepositoryContext context) : RepositoryBase<Employee>(context), IEmployeeRepository
{

}
