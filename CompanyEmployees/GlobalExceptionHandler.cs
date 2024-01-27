using Contracts;
using Entities;
using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace CompanyEmployees;

public class GlobalExceptionHandler(ILoggerManager logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
       var exceptionHandlerFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
       if(exceptionHandlerFeature != null)
       {
         httpContext.Response.ContentType = "application/json";
         logger.LogError($"There is an error in your application. {exception}");
         httpContext.Response.StatusCode = exception switch{
            NotFoundException => StatusCodes.Status404NotFound,
            BadRequestException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
         };
         await httpContext.Response.WriteAsync(new ErrorModel{
            Message = exception.Message,
            StatusCode = httpContext.Response.StatusCode
         }.ToString());
      
       }
        return true;
    }
}
