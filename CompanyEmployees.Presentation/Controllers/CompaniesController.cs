using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies")]
[ApiController]
public class CompaniesController(IServiceManager service) : ControllerBase
{
   [HttpGet]
   public async Task<IActionResult> GetCompanies()
   {
      var companies =  await service.CompanyService.GetCompaniesAsync(false);
      return Ok(companies);
   }

   [HttpGet("{id:guid}")]
   public async Task<IActionResult> GetCompany([FromRoute] Guid id)
   {
      var company = await service.CompanyService.GetCompanyAsync(id,false);
      return Ok(company);
   }
}
