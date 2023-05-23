
using System;

namespace BatteryMonitorApp.Contracts.Models.Http
{
    public class BatteryDataRequest
    {
        /// <summary>
        /// (DeviceId). Required field. Guid string; Like: 'DE88CE88-E888-8A88-8888-888888888888'
        /// </summary>
        public Guid Di { get; set; }
        /// <summary>
        /// (From). Optional parameter. DateTime
        /// </summary>
        public DateTime F { get; set; }= DateTime.MinValue;
        /// <summary>
        /// (To). Optional parameter. DateTime
        /// </summary>
        public DateTime T  { get; set; }=DateTime.MaxValue;
        /// <summary>
        /// (Array of Status). Optional parameter. int[]
        /// </summary>
        public int[] S { get; set; } = new int[] { 0,1,2,3,4,5,6,7,8,9 };
    }
}
