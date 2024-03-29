﻿using CompanyEmployees.Presentation.ModelBindings;
using Entities.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Service.Contracts;
using Shared;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies")]
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
public class CompaniesController(IServiceManager service) : ApiControllerBase
{
   [Authorize(Roles ="Manager")]
   [HttpGet]
   [HttpHead]
   [OutputCache(PolicyName ="120SecondsDuration")]
   public async Task<IActionResult> GetCompanies()
   {
      var baseResult =  await service.CompanyService.GetCompaniesAsync(false);
      var companies = ApiBaseResponseExtensions.GetResult<IEnumerable<CompanyDto>>(baseResult);
      var etag = $"\"{Guid.NewGuid():n}\"";
      HttpContext.Response.Headers.ETag = etag;
      return Ok(companies);
   }

   [HttpGet("{id:guid}", Name = "CompanyById")]
   public async Task<IActionResult> GetCompany([FromRoute] Guid id)
   {
      var baseResult = await service.CompanyService.GetCompanyAsync(id,false);
      if(!baseResult.Success)
      {
         return ProcessError(baseResult);
      }
      var company = ApiBaseResponseExtensions.GetResult<CompanyDto>(baseResult);
      return Ok(company);
   }

   [HttpPost]
   public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto companyForCreationDto)
   {
      var company = await service.CompanyService.CreateCompanyAsync(companyForCreationDto);
      return CreatedAtRoute("CompanyById",new{id=company.Id},company);
   }

   [HttpGet("collection/({ids})", Name = "CompanyCollection")]
   public async Task<IActionResult> GetCompanyByIds([ModelBinder(typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
   {
      var companies =  await service.CompanyService.GetCompanyByIdsAsync(ids,false);
      return Ok(companies);
   }

   [HttpPost("collection")]
   public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyForCreationDtos)
   {
      var company = await service.CompanyService.CreateCompanyCollectionAsync(companyForCreationDtos);
      return CreatedAtRoute("CompanyCollection",new{company.ids},company.companies);
   }

   [HttpDelete("{id:guid}")]
   public async Task<IActionResult> DeleteCompany([FromRoute]Guid id)
   {
      await service.CompanyService.DeleteCompanyAsync(id,false);
      return NoContent();
   }

   [HttpPut("{id:guid}")]
   public async Task<IActionResult> UpdateCompany([FromRoute] Guid id, [FromBody] CompanyForUpdateDto companyForUpdateDto)
   {
      await service.CompanyService.UpdateCompanyAsync(id, companyForUpdateDto, true);
      return NoContent();
   }

   [HttpOptions]
   public IActionResult GetCompaniesOptions()
   {
      Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT, DELETE");
      return Ok();
   }
}
