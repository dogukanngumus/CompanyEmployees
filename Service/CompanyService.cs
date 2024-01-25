using Contracts;
using Service.Contracts;

namespace Service;

public class CompanyService(IRepositoryManager repository, ILoggerManager logger) : ICompanyService
{

}
