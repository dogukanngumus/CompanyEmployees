namespace Contracts;

public interface IRepositoryManager
{
    public ICompanyRepository CompanyRepository { get;}
    public IEmployeeRepository EmployeeRepository { get;}
    Task SaveAsync();
}
