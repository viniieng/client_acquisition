using AutoMapper;
using ClientAcquisition.Application.Common.Interfaces;
using ClientAcquisition.Application.Customers;
using ClientAcquisition.Application.Orders;
using ClientAcquisition.Application.Reports;
using ClientAcquisition.Application.Mapping;
using ClientAcquisition.Infrastructure;
using ClientAcquisition.Infrastructure.Configuration;
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

// Accept both the Npgsql keyword format and a postgres:// URI (e.g. the one Supabase
// shows in its dashboard) by normalizing whatever was configured.
var resolvedConn = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrWhiteSpace(resolvedConn))
{
    builder.Configuration["ConnectionStrings:DefaultConnection"] =
        PostgresConnectionString.Normalize(resolvedConn);
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
    });
});

var app = builder.Build();


app.UseMiddleware<ClientAcquisition.Api.Middleware.ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply pending EF Core migrations at startup (development convenience)
try
{
    using (var scope = app.Services.CreateScope())
    {
        var appDb = scope.ServiceProvider.GetService<ClientAcquisition.Infrastructure.Persistence.ApplicationDbContext>();
        if (appDb != null)
        {
            appDb.Database.Migrate();
        }
    }
}
catch
{
    // Migration errors will be logged by the framework - do not crash startup here
}

app.UseCors("Frontend");
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program;