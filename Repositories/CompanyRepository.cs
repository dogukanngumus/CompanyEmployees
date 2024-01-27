using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class CompanyRepository(RepositoryContext context) : RepositoryBase<Company>(context), ICompanyRepository
{
    public async Task<IEnumerable<Company>> GetCompaniesAsync(bool trackChanges)
    => await FindAll(trackChanges).OrderBy(x=> x.Name).ToListAsync();
    public async Task<IEnumerable<Company>> GetCompanyByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
    => await FindByCondition(x=> ids.Contains(x.Id),trackChanges).OrderBy(x=> x.Name).ToListAsync();
    public async Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges)
    => await FindByCondition(x=> x.Id.Equals(companyId), trackChanges).FirstOrDefaultAsync();
    
    public void CreateCompany(Company company)
    => Create(company);

    public void DeleteCompany(Company company)
    => Delete(company);
}
