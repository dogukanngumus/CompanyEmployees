using AutoMapper;
using Contracts;
using Entities;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared;
using Shared.DataTransferObjects;

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

    public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto companyForCreationDto)
    {
       var company = mapper.Map<Company>(companyForCreationDto);

       repository.CompanyRepository.CreateCompany(company);
       await repository.SaveAsync();

       var companyDto = mapper.Map<CompanyDto>(company); 
       return companyDto;
    }
    
    public async Task<IEnumerable<CompanyDto>> GetCompanyByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
    {
        var companies = await repository.CompanyRepository.GetCompanyByIdsAsync(ids,trackChanges);
        var companyDtos = mapper.Map<IEnumerable<CompanyDto>>(companies);
        return companyDtos;
    }

    public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companies)
    {
        var entities = mapper.Map<IEnumerable<Company>>(companies);
        foreach(var entity in entities)
        {
            repository.CompanyRepository.CreateCompany(entity);
        }
        await repository.SaveAsync();
        return (mapper.Map<IEnumerable<CompanyDto>>(entities),string.Join(",",entities.Select(x=> x.Id)));
    }

    public async Task DeleteCompanyAsync(Guid id, bool trackChanges)
    {
       var company = await GetCompanyIfExists(id, trackChanges);

       repository.CompanyRepository.DeleteCompany(company);
       await repository.SaveAsync();
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

    public async Task UpdateCompanyAsync(Guid id, CompanyForUpdateDto companyForUpdateDto, bool trackChanges)
    {
        var company = await GetCompanyIfExists(id,trackChanges);
        mapper.Map(companyForUpdateDto, company);
        await repository.SaveAsync();
    }
}
