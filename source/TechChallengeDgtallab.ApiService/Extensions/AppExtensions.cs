using Microsoft.EntityFrameworkCore;
using TechChallengeDgtallab.Infra.Data;

namespace TechChallengeDgtallab.ApiService.Extensions;

public static class AppExtensions
{
    public static void ApplyDatabaseMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }
}