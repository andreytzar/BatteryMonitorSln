using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace BatteryMonitorApp.Domain.Models.DataBase
{
    public record BatteryRegisteredDevice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }= Guid.NewGuid();
        [Required]
        public Guid UserId { get; set; }
        [MaxLength(255)]
        [NotNull]
        public string DeviceName { get; set; } = string.Empty;
        [MaxLength(100)]
        [NotNull]
        public string DeviceDescription { get; set; } = string.Empty;
        [NotNull]
        public List<BatteryData> BatteryDatas { get; set; } = new();
    }
}
