using System.IO.Compression;
using CF.Api.Filters;
using CF.Api.Middleware;
using CF.Customer.Application.Facades;
using CF.Customer.Application.Facades.Interfaces;
using CF.Customer.Domain.Repositories;
using CF.Customer.Domain.Services;
using CF.Customer.Domain.Services.Interfaces;
using CF.Customer.Infrastructure.DbContext;
using CF.Customer.Infrastructure.Mappers;
using CF.Customer.Infrastructure.Repositories;
using CF.Insurance.Application.Facades;
using CF.Insurance.Application.Facades.Interfaces;
using CF.Insurance.Domain.Repositories;
using CF.Insurance.Domain.Services;
using CF.Insurance.Domain.Services.Interfaces;
using CF.Insurance.Infrastructure.DbContext;
using CF.Insurance.Infrastructure.Repositories;
using CF.Vehicle.Application.Facades;
using CF.Vehicle.Application.Facades.Interfaces;
using CF.Vehicle.Domain.Repositories;
using CF.Vehicle.Domain.Services;
using CF.Vehicle.Domain.Services.Interfaces;
using CF.Vehicle.Infrastructure.DbContext;
using CF.Vehicle.Infrastructure.Repositories;
using CorrelationId;
using CorrelationId.DependencyInjection;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.SwaggerGen;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLog();

builder.Services.AddControllers(x => x.Filters.Add<ExceptionFilter>());
builder.Services.AddProblemDetails();
builder.Services.AddDefaultCorrelationId(ConfigureCorrelationId());
builder.Services.AddTransient<ICustomerFacade, CustomerFacade>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();

builder.Services.AddTransient<IInsuranceFacade, InsuranceFacade>();
builder.Services.AddTransient<IInsuranceService, InsuranceService>();
builder.Services.AddTransient<IInsuranceRepository, InsuranceRepository>();

builder.Services.AddTransient<IVehicleFacade, VehicleFacade>();
builder.Services.AddTransient<IVehicleService, VehicleService>();
builder.Services.AddTransient<IVehicleRepository, VehicleRepository>();

builder.Services.AddTransient<IPasswordHasherService, PasswordHasherService>();
builder.Services.AddAutoMapper(typeof(CustomerProfile));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SetupSwagger());
builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
builder.Services.AddResponseCompression(options => { options.Providers.Add<GzipCompressionProvider>(); });
builder.Services.AddDbContext<CustomerContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"),
        a => { a.MigrationsAssembly("CF.Migrations"); });
});

builder.Services.AddDbContext<InsuranceContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"),
        a => { a.MigrationsAssembly("CF.Migrations"); });
});

builder.Services.AddDbContext<VehicleContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"),
        a => { a.MigrationsAssembly("CF.Migrations"); });
});

AddNLog();

await using var app = builder.Build();

RunMigration();
app.UseCorrelationId();
AddExceptionHandler();
AddSwagger();
app.UseMiddleware<LogExceptionMiddleware>();
app.UseMiddleware<LogRequestMiddleware>();
app.UseMiddleware<LogResponseMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

await app.RunAsync();

void AddExceptionHandler()
{
    if (app.Environment.IsDevelopment()) return;
    app.UseExceptionHandler(ConfigureExceptionHandler());
}

void AddSwagger()
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CF Api"));
}

void AddNLog()
{
    if (builder.Environment.EnvironmentName.Contains("Test")) return;
    LogManager.Setup().LoadConfigurationFromSection(builder.Configuration);
}

Action<SwaggerGenOptions> SetupSwagger()
{
    return c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "CF API", Version = "v1" });

        c.CustomSchemaIds(x => x.FullName);

        c.AddSecurityDefinition("Authorization", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            Description = "JWT Authorization header using the Bearer scheme",
            In = ParameterLocation.Header
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Authorization"
                    }
                },
                new List<string>()
            }
        });
    };
}

Action<CorrelationIdOptions> ConfigureCorrelationId()
{
    return options =>
    {
        options.LogLevelOptions = new CorrelationIdLogLevelOptions
        {
            FoundCorrelationIdHeader = LogLevel.Debug,
            MissingCorrelationIdHeader = LogLevel.Debug
        };
    };
}

Action<IApplicationBuilder> ConfigureExceptionHandler()
{
    return exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(new
            {
                Message = "An unexpected internal exception occurred."
            });
        });
    };
}

void RunMigration()
{
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

    if (!serviceScope.ServiceProvider.GetRequiredService<CustomerContext>().Database.GetPendingMigrations()
            .Any()) return;

    serviceScope.ServiceProvider.GetRequiredService<CustomerContext>().Database.Migrate();

    if (!serviceScope.ServiceProvider.GetRequiredService<InsuranceContext>().Database.GetPendingMigrations()
            .Any()) return;

    serviceScope.ServiceProvider.GetRequiredService<InsuranceContext>().Database.Migrate();

    if (!serviceScope.ServiceProvider.GetRequiredService<VehicleContext>().Database.GetPendingMigrations()
            .Any()) return;

    serviceScope.ServiceProvider.GetRequiredService<VehicleContext>().Database.Migrate();
}

public partial class Program
{
}