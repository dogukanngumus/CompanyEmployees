using Entities.Exceptions;

namespace Entities;

public sealed class EmployeeNotFoundException(Guid employeeId) : NotFoundException($"Employee entity with this is {employeeId} not found.")
{

}
