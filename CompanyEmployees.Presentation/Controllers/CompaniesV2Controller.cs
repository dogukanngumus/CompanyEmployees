using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Presentation;

[Route("api/companies")]
[ApiController]
public class CompaniesV2Controller : ControllerBase
{
   [HttpGet]
   public async Task<IActionResult> GetCompanies()
   {
      return Ok("GetCompanies V2.");
   }
}
