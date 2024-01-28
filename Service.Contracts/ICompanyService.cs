using Entities.Responses;
using Shared;
using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface ICompanyService
{
    Task<ApiBaseResponse> GetCompaniesAsync(bool trackChanges);
    Task<IEnumerable<CompanyDto>> GetCompanyByIdsAsync(IEnumerable<Guid> ids,bool trackChanges);
    Task<ApiBaseResponse> GetCompanyAsync(Guid companyId, bool trackChanges);
    Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto companyForCreationDto);
    Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companies);
    Task DeleteCompanyAsync(Guid id, bool trackChanges);
    Task UpdateCompanyAsync(Guid id,CompanyForUpdateDto companyForUpdateDto, bool trackChanges);
}
