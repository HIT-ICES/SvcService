using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SvcService.Data;

public class InterfaceEntity
{
    public ServiceEntity Service { get; set; }
    [ForeignKey(nameof(Service))]
    public string ServiceId { get; set; }
    [Required,MaxLength(32)]
    public string IdSuffix { get; set; }
    [Required, MaxLength(64)]
    public string Path { get; set; }
    public int InputSize { get; set; }
    [MaxLength(50)]
    public string OutputSize{ get; set; }
    [MaxLength(128)]
    public string Info { get; set; }
    [MaxLength(16)]
    public string Method { get; set; }
    public List<DependencyEntity> Callers { get; set; } = new();
    public List<DependencyEntity> Callees { get; set; } = new();

    [NotMapped]
    public string Id
    {
        get=> $"{ServiceId}::{IdSuffix}";
        set
        {
            var spl=value.Split("::");
            ServiceId = spl[0];
            IdSuffix = spl[1];
        }
    }
}