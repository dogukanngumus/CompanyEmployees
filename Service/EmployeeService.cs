using Contracts;
using Service.Contracts;

namespace Service;

public class EmployeeService(IRepositoryManager repository, ILoggerManager logger) : IEmployeeService
{

}
