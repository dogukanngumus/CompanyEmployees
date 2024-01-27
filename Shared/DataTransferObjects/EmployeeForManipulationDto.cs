using Shared.DataTransferObjects;

namespace Shared;

public record EmployeeForManipulationDto : IDto
{
    public string? Name { get; set; }
    public int Age { get; set; }
    public string? Position { get; set; }
}
