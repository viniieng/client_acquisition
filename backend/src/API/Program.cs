using AutoMapper;
using ClientAcquisition.Application.Common.Interfaces;
using ClientAcquisition.Application.Customers;
using ClientAcquisition.Application.Orders;
using ClientAcquisition.Application.Reports;
using ClientAcquisition.Application.Mapping;
using ClientAcquisition.Infrastructure;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;

// Load variables from a .env file (searching up to the repo root) so SUPABASE_CONNECTION_STRING
// works with `dotnet run`, matching the README. Existing process env vars take precedence.
DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// Allow overriding the connection string via environment variable (useful for Supabase)
var envConn = Environment.GetEnvironmentVariable("SUPABASE_CONNECTION_STRING")
              ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
if (!string.IsNullOrWhiteSpace(envConn))
{
    builder.Configuration["ConnectionStrings:DefaultConnection"] = envConn;
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Client Acquisition API",
        Version = "v1"
    });
});

builder.Services.AddSingleton<IMapper>(_ =>
{
    var configuration = new MapperConfiguration(configurationExpression =>
    {
        configurationExpression.AddProfile<MappingProfile>();
    });

    return configuration.CreateMapper();
});
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IReportService, ReportService>();

// Configure CORS to allow calls from the frontend dev server
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins(
                "http://localhost:8080",
                "https://localhost:8080"
            );

        var allowedOrigins = (
            Environment.GetEnvironmentVariable("AllowedOrigins")
            ?? builder.Configuration["AllowedOrigins"]
            ?? ""
        ).Split(",", StringSplitOptions.RemoveEmptyEntries);

        if (allowedOrigins.Length > 0)
            policy.WithOrigins(allowedOrigins);
    });
});
var app = builder.Build();


app.UseMiddleware<ClientAcquisition.Api.Middleware.ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply pending EF Core migrations at startup (development convenience). If the schema was
// created manually (e.g. directly in Supabase), the migration history is baselined instead of
// re-creating the existing tables.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var appDb = services.GetService<ClientAcquisition.Infrastructure.Persistence.ApplicationDbContext>();
    if (appDb != null)
    {
        try
        {
            ClientAcquisition.Api.DatabaseInitializer.Initialize(appDb, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database initialization failed at startup.");
        }
    }
}

app.UseCors("Frontend");
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program;