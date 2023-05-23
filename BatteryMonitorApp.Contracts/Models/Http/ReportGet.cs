
using System;
using System.Collections.Generic;

namespace BatteryMonitorApp.Contracts.Models.Http
{
    public record ReportGet
    {
        public Guid DeviceId { get; set; } = Guid.Empty;
        public DateTime From { get; set; }= DateTime.Now.AddDays(-1);
        public DateTime To { get; set; } = DateTime.Now;
        public double Capacity { get; set; }
        public List<NameGuidDevice> Devices { get; set; } = new();
        public List<BatteryDataView> BatteryDataViews { get; set; } = null;
    }
}
