using System.Text.Json;
using CompanyEmployees.Presentation.ActionFilters;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.RequestFeatures;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies/{companyId:guid}/employees")]
[ApiController]
public class EmployeesController(IServiceManager service) : ControllerBase
{
     [HttpGet]
     public async Task<IActionResult> GetCompanyEmployees([FromRoute] Guid companyId, [FromQuery]EmployeeParameters employeeParameters)
     {
        var employees = await service.EmployeeService.GetCompanyEmployeesAsync(companyId,employeeParameters,false);
        Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(employees.metaData));
        return Ok(employees.employees);
     }

    [HttpGet("{employeeId:guid}",Name ="EmployeeWithId")]
     public async Task<IActionResult> GetEmployee([FromRoute] Guid companyId, [FromRoute] Guid employeeId)
     {
        var employee = await service.EmployeeService.GetCompanyEmployeeAsync(companyId, employeeId, false);
        return Ok(employee);
     }

     [HttpPost]
     [ServiceFilter(typeof(ValidationFilterAttribute))]
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
     [ServiceFilter(typeof(ValidationFilterAttribute))]
     public async Task<IActionResult> UpdateEmployee([FromRoute] Guid companyId,[FromRoute] Guid employeeId, [FromBody] EmployeeForUpdateDto employeeForUpdateDto)
     {
        await service.EmployeeService.UpdateEmployeeAsync(companyId, employeeId,employeeForUpdateDto, false, true);
        return NoContent();
     }

     [HttpPatch("{employeeId:guid}")]
     public async Task<IActionResult> PatchEmployee([FromRoute]Guid companyId, [FromRoute]Guid employeeId ,[FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchdoc)
     {
        var result = await service.EmployeeService.GetEmployeeForPatchAsync(companyId, employeeId,false,true);

        patchdoc.ApplyTo(result.employeeForUpdate, ModelState);

        TryValidateModel(result.employeeForUpdate);

        if(!ModelState.IsValid)
        {
           return UnprocessableEntity();
        }

        await service.EmployeeService.SaveChangesForPatchAsync(result.employeeForUpdate, result.employee);
        return NoContent();
     }
}
