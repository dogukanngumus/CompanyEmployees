using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class CompanyRepository(RepositoryContext context) : RepositoryBase<Company>(context), ICompanyRepository
{
    public async Task<IEnumerable<Company>> GetCompaniesAsync(bool trackChanges)
    => FindAll(trackChanges).OrderBy(x=> x.Name).ToList();

    public async Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges)
    => await FindByCondition(x=> x.Id.Equals(companyId), trackChanges).FirstOrDefaultAsync();
}
