namespace HeadCode.DataAccess.DatabaseContexts;

using Core.HelpingModels;
using Core.Models;
using Microsoft.EntityFrameworkCore;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
        
    }
    
    public DbSet<Problem> Problems { get; set; }
    public DbSet<ProblemTest> ProblemTests { get; set; }
    
    public DbSet<Solution> Solutions { get; set; }
    
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connectionString = "";

        optionsBuilder.UseNpgsql(connectionString)
                      .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}