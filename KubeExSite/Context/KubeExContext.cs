using KubeExSite.Context.Models;
using Microsoft.EntityFrameworkCore;

namespace KubeExSite.Context;

public class KubeExContext : DbContext
{
    public KubeExContext(DbContextOptions<KubeExContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }
    
    public virtual DbSet<RoutineReport> RoutineReports { get; set; }
}