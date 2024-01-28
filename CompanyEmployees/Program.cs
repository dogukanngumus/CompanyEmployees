using CompanyEmployees.Extensions;
using CompanyEmployees.Presentation;
using CompanyEmployees.Presentation.ActionFilters;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using NLog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(options=>{
    options.RespectBrowserAcceptHeader = true;
    options.ReturnHttpNotAcceptable = true;
    options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
})
.AddXmlDataContractSerializerFormatters()
.AddCustomCsvFormatter()
.AddApplicationPart(typeof(AssemblyReference).Assembly);

NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() => new ServiceCollection().AddLogging()
.AddMvc().AddNewtonsoftJson().Services.BuildServiceProvider()
.GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters.OfType<NewtonsoftJsonPatchInputFormatter>().First();

builder.Services.Configure<ApiBehaviorOptions>(options=>{
    options.SuppressModelStateInvalidFilter = true;
});

#region Configurations
LogManager.Setup().LoadConfigurationFromFile();
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureAutoMapper();
builder.Services.ConfigureExceptionHandler();
builder.Services.ConfigureDataShaper();
builder.Services.ConfigureEmployeeLinks();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureOutputCaching();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddCustomMediaTypes();
builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<ValidateMediaTypeAttribute>();
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);
#endregion

var app = builder.Build();

app.UseExceptionHandler(options=>{});

if(app.Environment.IsProduction())
{
    app.UseHsts();
}
else
{
   // swagger
}
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions(){
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseRateLimiter();
app.UseCors("CorsPolicy");
app.UseOutputCache();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();