using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies")]
[ApiController]
[ApiExplorerSettings(GroupName = "v2")]
public class CompaniesV2Controller : ControllerBase
{
   [HttpGet]
   [HttpHead]
   public async Task<IActionResult> GetCompanies()
   {
      return Ok("GetCompanies V2 started.");
   }
}
