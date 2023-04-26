using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SvcService.Data;

public class InterfaceEntity
{
    record Interface(string Id, string Path, int InputSize, string OutputSize);
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