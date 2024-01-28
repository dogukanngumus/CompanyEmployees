using Microsoft.AspNetCore.Http;
using Shared.RequestFeatures;

namespace Entities;

public record LinkParameters(EmployeeParameters EmployeeParameters, HttpContext Context);
