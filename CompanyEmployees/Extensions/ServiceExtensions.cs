using System.Text;
using System.Threading.RateLimiting;
using Asp.Versioning;
using CompanyEmployees.Presentation;
using CompanyEmployees.Presentation.Controllers;
using CompanyEmployees.Utility;
using Contracts;
using Entities.Models;
using LoggerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.IdentityModel.Tokens;
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

    public static void ConfigureOutputCaching(this IServiceCollection services)
    => services.AddOutputCache(opt=>{
        opt.AddPolicy("120SecondsDuration", p => p.Expire(TimeSpan.FromSeconds(120)));
    });

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

    public static void ConfigureVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions = true;
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.DefaultApiVersion = new ApiVersion(1, 0);
            opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
        }).AddMvc(opt=>{
            opt.Conventions.Controller<CompaniesController>().HasApiVersion(new ApiVersion(1,0));
            opt.Conventions.Controller<EmployeesController>().HasApiVersion(new ApiVersion(1,0));
            opt.Conventions.Controller<CompaniesV2Controller>().HasDeprecatedApiVersion(new ApiVersion(2,0));
        });
    }

    public static void ConfigureRateLimitingOptions(this IServiceCollection services)
    {
        services.AddRateLimiter(opt =>
        {
            opt.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context
            => RateLimitPartition.GetFixedWindowLimiter("GlobalLimiter",partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 5,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));
            opt.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = 429;
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)){
                    await context.HttpContext.Response.WriteAsync($"Too many requests. Please try again after {retryAfter.TotalSeconds} second(s).", token);
                }            
                else
                {
                    await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", token);
                };
            };

            opt.AddPolicy("SpecificPolicy", context => RateLimitPartition.GetFixedWindowLimiter("SpecificLimiter",
            partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 3,
                Window = TimeSpan.FromSeconds(10)
            }));

        });
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentity<User, IdentityRole>(o =>
        {
            o.Password.RequireDigit = true;
            o.Password.RequireLowercase = false;
            o.Password.RequireUppercase = false;
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequiredLength = 10;
            o.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<RepositoryContext>().AddDefaultTokenProviders();
    }

    public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["validIssuer"],
                ValidAudience = jwtSettings["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["secretKey"]))
            }; 
        });
    }
}
