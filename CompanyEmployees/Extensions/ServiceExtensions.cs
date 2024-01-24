﻿using Contracts;
using LoggerService;
using Repositories;

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
}