namespace Shared.DataTransferObjects;

public abstract record CompanyForManipulationDto : IDto
{
    public string Name { get; init; }
    public string Address { get; init; }
    public string Country { get; init; }
    public IEnumerable<EmployeeForCreationDto> Employees { get; set; }
}
