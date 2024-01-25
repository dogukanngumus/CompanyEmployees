namespace Shared;

public record class CompanyDto
{
   public Guid Id { get; set; }
   public string? Name { get; set; }
   public string? FullAddress { get; set; }
}
