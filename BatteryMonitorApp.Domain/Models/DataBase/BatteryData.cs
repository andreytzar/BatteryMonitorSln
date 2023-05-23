using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace BatteryMonitorApp.Domain.Models.DataBase
{
    [Table("Batteries_Data")]
    public record BatteryData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        [NotNull]
        public Guid DeviceId { get; set; }
        [Required]
        [AllowNull]
        public BatteryRegisteredDevice Device { get; set; }
        [Required]
        [NotNull]
        public float Voltage { get; set; }
        [NotNull]
        public float Current { get; set; }
        [NotNull]
        public float VoltageCharger { get; set; }
        [NotNull]
        public DateTime DateTime { get; set; } = DateTime.Now;
        [NotNull]
        public int Status { get; set; } = 0;
    }


}
