using System.Text.Json;
using CompanyEmployees.Presentation.ActionFilters;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.RequestFeatures;
using Shared.DataTransferObjects;
using Entities;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies/{companyId:guid}/employees")]
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
public class EmployeesController(IServiceManager service) : ControllerBase
{
     [HttpGet(Name = "GetEmployeesForCompany")]
     [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
     public async Task<IActionResult> GetEmployeesForCompany([FromRoute] Guid companyId, [FromQuery]EmployeeParameters employeeParameters)
     {
       var linkParams = new LinkParameters(employeeParameters, HttpContext);
       var result = await service.EmployeeService.GetCompanyEmployeesAsync(companyId,linkParams, trackChanges: false);
       Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(result.metaData));
       return result.linkResponse.HasLinks ? Ok(result.linkResponse.LinkedEntities) : Ok(result.linkResponse.ShapedEntities);
     }

    [HttpGet("{employeeId:guid}",Name ="GetEmployeeForCompany")]
     public async Task<IActionResult> GetEmployeeForCompany([FromRoute] Guid companyId, [FromRoute] Guid employeeId)
     {
        var employee = await service.EmployeeService.GetCompanyEmployeeAsync(companyId, employeeId, false);
        return Ok(employee);
     }

     [HttpPost]
     [ServiceFilter(typeof(ValidationFilterAttribute))]
     public async Task<IActionResult> CreateEmployee([FromRoute] Guid companyId, [FromBody] EmployeeForCreationDto employeeForCreationDto)
     {
       var employee = await service.EmployeeService.CreateEmployeeAsync(companyId,employeeForCreationDto,false);
       return CreatedAtRoute("GetEmployeeForCompany",new{companyId,employee.Id},employee);
     }

     [HttpDelete("{employeeId:guid}")]
     public async Task<IActionResult> DeleteEmployeeForCompany([FromRoute] Guid companyId, [FromRoute] Guid employeeId)
     {
        await service.EmployeeService.DeleteEmployeeAsync(companyId, employeeId,false);
        return NoContent();
     }
     
     [HttpPut("{employeeId:guid}")]
     [ServiceFilter(typeof(ValidationFilterAttribute))]
     public async Task<IActionResult> UpdateEmployeeForCompany([FromRoute] Guid companyId,[FromRoute] Guid employeeId, [FromBody] EmployeeForUpdateDto employeeForUpdateDto)
     {
        await service.EmployeeService.UpdateEmployeeAsync(companyId, employeeId,employeeForUpdateDto, false, true);
        return NoContent();
     }

     [HttpPatch("{employeeId:guid}")]
     public async Task<IActionResult> PartiallyUpdateEmployeeForCompany([FromRoute]Guid companyId, [FromRoute]Guid employeeId ,[FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchdoc)
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
