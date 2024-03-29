﻿using Contracts;
using Entities;
using Entities.LinkModels;
using Entities.Models;
using Microsoft.Net.Http.Headers;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Utility;

public class EmployeeLinks : IEmployeeLinks
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IDataShaper<EmployeeDto> _dataShaper;
    public EmployeeLinks(LinkGenerator linkGenerator, IDataShaper<EmployeeDto> dataShaper)
    {
        _linkGenerator = linkGenerator;
        _dataShaper = dataShaper;
    }
    public LinkResponse TryGenerateLinks(IEnumerable<EmployeeDto> employeesDto, string fields, Guid companyId, HttpContext httpContext)
    {
        var shapedEmployees = ShapeEntities(employeesDto, fields);
        if(ShouldGenerateLinks(httpContext))
        {
            return ReturnLinkdedEmployees(employeesDto, fields, companyId, httpContext,shapedEmployees);
        }
        return new LinkResponse(){ShapedEntities = shapedEmployees};
    }

    private List<Entity> ShapeEntities(IEnumerable<EmployeeDto> employeesDto, string fields)
    {
        return _dataShaper.GetShapedEntities(employeesDto, fields).Select(x=> x.Entity).ToList();
    }

    private bool ShouldGenerateLinks(HttpContext httpContext)
    {
        var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
        return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
    }

    private LinkResponse ReturnLinkdedEmployees(IEnumerable<EmployeeDto> employeesDto, string fields, Guid companyId
    , HttpContext httpContext, List<Entity> shapedEmployees)
    {
        var employeesDtoList = employeesDto.ToList();
        for (var index = 0; index < employeesDtoList.Count(); index++)
        {
            var employeeLinks = CreateLinksForEmployee(httpContext, companyId,employeesDtoList[index].Id, fields);
            shapedEmployees[index].Add("Links", employeeLinks);
        }
        
        var employeeCollection = new LinkCollectionWrapper<Entity>(shapedEmployees);
        var linkedEmployees = CreateLinksForEmployees(httpContext, employeeCollection);
        return new LinkResponse { HasLinks = true, LinkedEntities = linkedEmployees };
    }

    private List<Link> CreateLinksForEmployee(HttpContext httpContext, Guid companyId,Guid employeeId, string fields = "")
    {
        var links = new List<Link>
        {
            new (_linkGenerator.GetUriByAction(httpContext, "GetEmployeeForCompany",values: new { companyId, employeeId, fields }),"self","GET"),
            new (_linkGenerator.GetUriByAction(httpContext,"DeleteEmployeeForCompany", values: new { companyId, employeeId }),"delete_employee","DELETE"),
            new(_linkGenerator.GetUriByAction(httpContext,"UpdateEmployeeForCompany", values: new { companyId, employeeId }),"update_employee","PUT"),
            new(_linkGenerator.GetUriByAction(httpContext,"PartiallyUpdateEmployeeForCompany", values: new { companyId, employeeId }),"partially_update_employee","PATCH")
        };
        return links;
    }


    private LinkCollectionWrapper<Entity> CreateLinksForEmployees(HttpContext httpContext,LinkCollectionWrapper<Entity> employeesWrapper)
    {
        employeesWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext,"GetEmployeesForCompany", values: new { }),"self","GET"));
        return employeesWrapper;
    }
}
