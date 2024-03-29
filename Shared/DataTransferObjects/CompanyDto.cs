﻿namespace Shared.DataTransferObjects;

public record class CompanyDto : IDto
{
   public Guid Id { get; set; }
   public string? Name { get; set; }
   public string? FullAddress { get; set; }
}
