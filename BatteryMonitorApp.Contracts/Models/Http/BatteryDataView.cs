using System;


namespace BatteryMonitorApp.Contracts.Models.Http
{
    public class BatteryDataView
    {
        public float V { get; set; }
        public float C { get; set; }
        public float VC { get; set; }
        public DateTime? DT { get;  set; } = DateTime.Now;
        public BatteryEventStatus? S { get;  set; } = BatteryEventStatus.Default;
    }
}
