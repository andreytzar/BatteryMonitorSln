
using System;

namespace BatteryMonitorApp.Contracts.Models.Http
{

    public record NameGuidDevice
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
    }
}
