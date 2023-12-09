using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Steeltoe.Extensions.Configuration;

namespace SvcService.Data;

public class DependencyEntity
{
    //caller and callee
    [ForeignKey(nameof(CallerService))]
    public string CallerServiceId { get; set; }
    public ServiceEntity CallerService { get; set; }
    [Required,  MaxLength(128)]
    public string CallerIdSuffix { get; set; }
    public InterfaceEntity Caller { get; set; }

    [ForeignKey(nameof(CalleeService))]
    public string CalleeServiceId { get; set; }
    public ServiceEntity CalleeService { get; set; }
    [Required,  MaxLength(128)]
    public string CalleeIdSuffix { get; set; }
    public InterfaceEntity Callee { get; set; }
    [MaxLength(4096)]
    public string SerilizedData { get; set; }
    [NotMapped]
    public string CallerId
    {
        get => $"{CallerServiceId}::{CallerIdSuffix}";
        set
        {
            var spl = value.Split("::");
            CallerServiceId = spl[0];
            CallerIdSuffix = spl[1];
        }
    }
    [NotMapped]
    public string CalleeId
    {
        get => $"{CalleeServiceId}::{CalleeIdSuffix}";
        set
        {
            var spl = value.Split("::");
            CalleeServiceId = spl[0];
            CalleeIdSuffix = spl[1];
        }
    }
}