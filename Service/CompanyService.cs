using AutoMapper;
using Contracts;
using Entities;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared;

namespace Service;

public class CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : ICompanyService
{
    public async Task<IEnumerable<CompanyDto>> GetCompaniesAsync(bool trackChanges)
    {
       var companies = await repository.CompanyRepository.GetCompaniesAsync(trackChanges);
       var companyDtos = mapper.Map<IEnumerable<CompanyDto>>(companies);
       return companyDtos;
    }

    public async Task<CompanyDto> GetCompanyAsync(Guid companyId, bool trackChanges)
    {
        var company = await GetCompanyIfExists(companyId, trackChanges);
        var companyDto = mapper.Map<CompanyDto>(company);
        return companyDto;
    }
    
    private async Task<Company> GetCompanyIfExists(Guid companyId, bool trackChanges)
    {
        var company = await repository.CompanyRepository.GetCompanyAsync(companyId, trackChanges);
        if(company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }
        return company;
    }
}
