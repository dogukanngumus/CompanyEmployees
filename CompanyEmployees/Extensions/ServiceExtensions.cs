using Contracts;
using LoggerService;
using Repositories;
using Service;
using Service.Contracts;

namespace CompanyEmployees.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    => services.AddCors(opt=>{
        opt.AddPolicy("CorsPolicy",cfg=>{
            cfg.AllowAnyHeader();
            cfg.AllowAnyMethod();
            cfg.AllowAnyOrigin();
        });
    });

    public static void ConfigureIISIntegration(this IServiceCollection services)
    => services.Configure<IISOptions>(opt=>{});

    public static void ConfigureLoggerService(this IServiceCollection services)
    => services.AddScoped<ILoggerManager, LoggerManager>();

    public static void ConfigureRepositoryManager(this IServiceCollection services)
    => services.AddScoped<IRepositoryManager, RepositoryManager>();

    public static void ConfigureServiceManager(this IServiceCollection services)
    => services.AddScoped<IServiceManager, ServiceManager>();

    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
    => services.AddSqlServer<RepositoryContext>(configuration.GetConnectionString("sqlServer"));
}
