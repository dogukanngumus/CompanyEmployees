using CompanyEmployees.Extensions;
using CompanyEmployees.Presentation;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddApplicationPart(typeof(AssemblyReference).Assembly);

#region Configurations
LogManager.Setup().LoadConfigurationFromFile();
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSqlContext(builder.Configuration);
#endregion

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions(){
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseCors("CorsPolicy");

app.MapControllers();
app.Run();