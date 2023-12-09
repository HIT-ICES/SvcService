using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SvcService.Data;
#pragma warning disable CS8618 // NotNull property not initialized
[Index(nameof(Name))]
public class ServiceEntity
{
    [Key,MaxLength(32)]
    public string Id { get; set; }
    [MaxLength(32), Required]
    public string Name { get; set; }
    [MaxLength(128), Required]
    public string Repo{ get; set; }
    [MaxLength(256), Required]
    public string ImageUrl { get; set; }
    public bool HasVersion { get; set; }
    [NotMapped] public Version? Version
    {
        get => HasVersion ? null : new(VersionMajor, VersionMinor, VersionPatch);
        set
        {
            if (value is null)
            {
                HasVersion = false;
                return;
            }

            VersionMajor = value.Major;
            VersionMinor = value.Minor;
            VersionPatch = value.Patch;
        }
    }

    [MaxLength(16)]
    public string VersionMajor { get; set; }
    [MaxLength(16)]
    public string VersionMinor { get; set; }
    [MaxLength(32)]
    public string VersionPatch { get; set; }

    [NotMapped]
    public Resource? IdleResource
    {
        get => HasIdleResource
            ? null
            : new(IdleCpu, IdleRam, IdleDisk, IdleGpuCore, IdleGpuMem);
        set
        {
            if (value is null)
            {
                HasIdleResource = false;
                return;
            }
            IdleDisk = value.Disk;
            IdleCpu = value.Cpu;
            IdleGpuCore = value.GpuCore;
            IdleGpuMem = value.GpuMem;
            IdleRam = value.Ram;
        }
    }
    public bool HasIdleResource{ get; set; }
    [Column(TypeName = "decimal(16,4)")]
    public decimal IdleCpu { get; set; }
    [Column(TypeName = "decimal(16,4)")]
    public decimal IdleRam { get; set; }
    [Column(TypeName = "decimal(16,4)")]
    public decimal IdleDisk { get; set; }
    [Column(TypeName = "decimal(16,4)")]
    public decimal IdleGpuCore { get; set; }
    [Column(TypeName = "decimal(16,4)")]
    public decimal IdleGpuMem { get; set; }
    [Column(TypeName = "decimal(16,4)"), Required]
    [NotMapped]
    public Resource DesiredResource
    {
        get => new(DesiredCpu, DesiredRam, DesiredDisk, DesiredGpuCore, DesiredGpuMem);
        set
        {
            DesiredDisk = value.Disk;
            DesiredCpu = value.Cpu;
            DesiredGpuCore = value.GpuCore;
            DesiredGpuMem = value.GpuMem;
            DesiredRam = value.Ram;
        }
    }
    [Column(TypeName = "decimal(16,4)"), Required]
    public decimal DesiredCpu { get; set; }
    [Column(TypeName = "decimal(16,4)"), Required]
    public decimal DesiredRam { get; set; }
    [Column(TypeName = "decimal(16,4)"), Required]
    public decimal DesiredDisk { get; set; }
    [Column(TypeName = "decimal(16,4)"), Required]
    public decimal DesiredGpuCore { get; set; }
    [Column(TypeName = "decimal(16,4)"), Required]
    public decimal DesiredGpuMem { get; set; }
    [Required]
    public int DesiredCapability{ get; set; }
    public List<InterfaceEntity> Interfaces{ get; set; }
    public List<DependencyEntity> Callers { get; set; } = new();
    public List<DependencyEntity> Callees { get; set; } = new();
}
#pragma warning restore CS8618 // NotNull property not initialized