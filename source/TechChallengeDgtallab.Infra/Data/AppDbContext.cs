using Microsoft.EntityFrameworkCore;
using TechChallengeDgtallab.Core.Models;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.Infra.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Collaborator> Collaborators { get; set; }
    public DbSet<Department> Departments { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}