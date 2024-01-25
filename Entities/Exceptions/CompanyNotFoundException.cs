namespace Entities.Exceptions;

public sealed class CompanyNotFoundException(Guid companyId):NotFoundException($"Company entity with this id : {companyId} does not exists on database ")
{

}
