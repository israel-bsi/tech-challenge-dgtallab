using Microsoft.EntityFrameworkCore;
using TechChallengeDgtallab.ApiService.Handlers;
using TechChallengeDgtallab.ApiService.Services;
using TechChallengeDgtallab.Core;
using TechChallengeDgtallab.Core.Handler;
using TechChallengeDgtallab.Core.Repositories;
using TechChallengeDgtallab.Infra.Data;
using TechChallengeDgtallab.Infra.Repositories;

namespace TechChallengeDgtallab.ApiService.Extensions;

public static class BuilderExtensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();
        builder.Services.AddProblemDetails();
        builder.Services.AddOpenApi();

        builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        builder.Services.AddScoped<ICollaboratorRepository, CollaboratorsRepository>();

        builder.Services.AddScoped<IDepartmentHandler, DepartmentHandler>();
        builder.Services.AddScoped<ICollaboratorHandler, CollaboratorHandler>();
        
        builder.Services.AddScoped<CollaboratorService>();
        builder.Services.AddScoped<DepartmentService>();
    }

    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        Configuration.ConnectionString =
            builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

        builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
    }

    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("TechChallengeDgtallab.ApiService"))
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging();
        });
    }

    public static void AddDocumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.CustomSchemaIds(n => n.FullName);
        });
    }
}