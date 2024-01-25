using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace CompanyEmployees.Presentation;

[Route("api/companies/{companyId:guid}/employees")]
[ApiController]
public class EmployeesController(IServiceManager service) : ControllerBase
{
     [HttpGet]
     public async Task<IActionResult> GetCompanyEmployees([FromRoute] Guid companyId)
     {
        var employees = await service.EmployeeService.GetCompanyEmployeesAsync(companyId,false);
        return Ok(employees);
     }

    [HttpGet("{employeeId:guid}")]
     public async Task<IActionResult> GetCompany([FromRoute] Guid companyId, [FromRoute] Guid employeeId)
     {
        var employee = await service.EmployeeService.GetCompanyEmployeeAsync(companyId, employeeId, false);
        return Ok(employee);
     }
}
