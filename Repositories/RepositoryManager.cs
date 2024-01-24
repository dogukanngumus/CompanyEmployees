using Contracts;

namespace Repositories;

public class RepositoryManager(RepositoryContext context) : IRepositoryManager
{
    private readonly Lazy<ICompanyRepository> _companyRepository = new(()=> new CompanyRepository(context));
    private readonly Lazy<IEmployeeRepository> _employeeRepository = new(()=> new EmployeeRepository(context));    
    public ICompanyRepository CompanyRepository => _companyRepository.Value;
    public IEmployeeRepository EmployeeRepository => _employeeRepository.Value;
    public async Task SaveAsync() => await context.SaveChangesAsync();
}
