using Microsoft.EntityFrameworkCore;

namespace SvcService.Data;

public class ServiceDbContext : DbContext
{
    public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options){}
    public DbSet<ServiceEntity> Services { get; set; }
    public DbSet<InterfaceEntity> Interfaces { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ServiceEntity>()
            .HasMany(s => s.Interfaces)
            .WithOne(i => i.Service);

        builder.Entity<InterfaceEntity>()
            .HasKey(i => new { i.ServiceId, i.IdSuffix });

    }
}