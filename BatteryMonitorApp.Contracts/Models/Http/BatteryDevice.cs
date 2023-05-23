using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;


namespace BatteryMonitorApp.Contracts.Models.Http
{
    public record BatteryDevice
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(255)]
        public string DeviceName { get; set; } = string.Empty;
        [MaxLength(100)]
        public string DeviceDescription { get; set; } = string.Empty;
    }
}
