using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

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

    [HttpGet("{employeeId:guid}",Name ="EmployeeWithId")]
     public async Task<IActionResult> GetEmployee([FromRoute] Guid companyId, [FromRoute] Guid employeeId)
     {
        var employee = await service.EmployeeService.GetCompanyEmployeeAsync(companyId, employeeId, false);
        return Ok(employee);
     }

     [HttpPost]
     public async Task<IActionResult> CreateEmployee([FromRoute] Guid companyId, [FromBody] EmployeeForCreationDto employeeForCreationDto)
     {
       var employee = await service.EmployeeService.CreateEmployeeAsync(companyId,employeeForCreationDto,false);
       return CreatedAtRoute("EmployeeWithId",new{companyId,employee.Id},employee);
     }

     [HttpDelete("{employeeId:guid}")]
     public async Task<IActionResult> DeleteEmployee([FromRoute] Guid companyId, [FromRoute] Guid employeeId)
     {
        await service.EmployeeService.DeleteEmployeeAsync(companyId, employeeId,false);
        return NoContent();
     }
     
     [HttpPut("{employeeId:guid}")]
     public async Task<IActionResult> UpdateEmployee([FromRoute] Guid companyId,[FromRoute] Guid employeeId, [FromBody] EmployeeForUpdateDto employeeForUpdateDto)
     {
        await service.EmployeeService.UpdateEmployeeAsync(companyId, employeeId,employeeForUpdateDto, false, true);
        return NoContent();
     }
}
