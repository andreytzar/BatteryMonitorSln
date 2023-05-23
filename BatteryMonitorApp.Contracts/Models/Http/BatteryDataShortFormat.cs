
using System;

namespace BatteryMonitorApp.Contracts.Models.Http
{
    public record BatteryDataShortFormat
    {
        public Guid Di { get; init; }=new Guid("DE88CE88-E888-8A88-8888-888888888888");
        public float V { get; init; } = 0;
        public float? C { get; init; } = 0;
        public float? VC { get; init; } = 0;
        public DateTime? Dt { get; init; } = DateTime.Now;
        public int? S { get; init; } = (int)BatteryEventStatus.Default;
    }
}
