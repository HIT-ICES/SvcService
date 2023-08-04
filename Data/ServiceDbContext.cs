using Microsoft.EntityFrameworkCore;

namespace SvcService.Data;

public class ServiceDbContext : DbContext
{
    public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options) { }
    public DbSet<ServiceEntity> Services { get; set; }
    public DbSet<InterfaceEntity> Interfaces { get; set; }
    public DbSet<DependencyEntity> Dependencies { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ServiceEntity>()
            .HasMany(s => s.Interfaces)
            .WithOne(i => i.Service);

        builder.Entity<ServiceEntity>()
            .HasMany(s => s.Callers)
            .WithOne(d => d.CalleeService);

        builder.Entity<ServiceEntity>()
            .HasMany(s => s.Callees)
            .WithOne(d => d.CallerService);

        builder.Entity<InterfaceEntity>()
            .HasKey(i => new { i.ServiceId, i.IdSuffix });

        builder.Entity<DependencyEntity>()
            .HasKey(d => new
            {
                d.CallerServiceId,
                d.CallerIdSuffix,
                d.CalleeServiceId,
                d.CalleeIdSuffix
            });

        builder.Entity<DependencyEntity>()
            .HasOne(d => d.Caller)
            .WithMany(i => i.Callees)
            .HasForeignKey(d => new { d.CallerServiceId, d.CallerIdSuffix });

        builder.Entity<DependencyEntity>()
            .HasOne(d => d.Callee)
            .WithMany(i => i.Callers)
            .HasForeignKey(d => new { d.CalleeServiceId, d.CalleeIdSuffix });

    }
}