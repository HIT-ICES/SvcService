using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SvcService.Data;

public class ServiceEntity
{
    [Key,MaxLength(32)]
    public string Id { get; set; }
    [MaxLength(128), Required]
    public string Repo{ get; set; }
    [MaxLength(16)]
    public string? VersionMajor { get; set; }
    [MaxLength(16)]
    public string? VersionMinor { get; set; }
    [MaxLength(32)]
    public string? VersionPatch { get; set; }
    [Column(TypeName = "decimal(16,4)")]
    public decimal? IdleCpu { get; set; }
    [Column(TypeName = "decimal(16,4)")]
    public decimal? IdleRam { get; set; }
    [Column(TypeName = "decimal(16,4)")]
    public decimal? IdleDisk { get; set; }
    [Column(TypeName = "decimal(16,4)")]
    public decimal? IdleGpuCore { get; set; }
    [Column(TypeName = "decimal(16,4)")]
    public decimal? IdleGpuMem { get; set; }
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
}