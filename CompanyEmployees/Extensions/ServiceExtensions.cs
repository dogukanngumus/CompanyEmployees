using CompanyEmployees.Utility;
using Contracts;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Repositories;
using Service;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    => services.AddCors(opt=>{
        opt.AddPolicy("CorsPolicy",cfg=>{
            cfg.AllowAnyHeader();
            cfg.AllowAnyMethod();
            cfg.AllowAnyOrigin();
            cfg.WithExposedHeaders("X-Pagination");
        });
    });

    public static void ConfigureIISIntegration(this IServiceCollection services)
    => services.Configure<IISOptions>(opt=>{});

    public static void ConfigureLoggerService(this IServiceCollection services)
    => services.AddSingleton<ILoggerManager, LoggerManager>();

    public static void ConfigureRepositoryManager(this IServiceCollection services)
    => services.AddScoped<IRepositoryManager, RepositoryManager>();

    public static void ConfigureServiceManager(this IServiceCollection services)
    => services.AddScoped<IServiceManager, ServiceManager>();

    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
    => services.AddSqlServer<RepositoryContext>(configuration.GetConnectionString("sqlServer"));

    public static void ConfigureAutoMapper(this IServiceCollection services)
    => services.AddAutoMapper(typeof(MappingProfiles));

    public static void ConfigureExceptionHandler(this IServiceCollection services)
    => services.AddExceptionHandler<GlobalExceptionHandler>();

    public static void ConfigureDataShaper(this IServiceCollection services)
    => services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();

     public static void ConfigureEmployeeLinks(this IServiceCollection services)
    => services.AddScoped<IEmployeeLinks, EmployeeLinks>();

    public static IMvcBuilder AddCustomCsvFormatter(this IMvcBuilder builder)
    => builder.AddMvcOptions(options=> options.OutputFormatters.Add(new CustomCsvOutputFormatter()));

    public static void AddCustomMediaTypes(this IServiceCollection services )
    {
        services.Configure<MvcOptions>(config =>
        {
            var systemTextJsonOutputFormatter = config.OutputFormatters
            .OfType<SystemTextJsonOutputFormatter>()?
            .FirstOrDefault();
            if (systemTextJsonOutputFormatter != null)
            {
                systemTextJsonOutputFormatter.SupportedMediaTypes
                .Add("application/vnd.companyemployees.hateoas+json");
            }


            var xmlOutputFormatter = config.OutputFormatters
            .OfType<XmlDataContractSerializerOutputFormatter>()?
            .FirstOrDefault();
            if (xmlOutputFormatter != null)
            {
                xmlOutputFormatter.SupportedMediaTypes
                .Add("application/vnd.companyemployees.hateoas+xml");
            }
        });
    }
}
