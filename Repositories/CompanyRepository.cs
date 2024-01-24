using Contracts;
using Entities.Models;

namespace Repositories;

public class CompanyRepository(RepositoryContext context) : RepositoryBase<Company>(context), ICompanyRepository
{

}
