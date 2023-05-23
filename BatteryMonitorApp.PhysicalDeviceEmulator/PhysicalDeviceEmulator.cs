using BatteryMonitorApp.Contracts.Models.Http;
using BatteryMonitorApp.Domain.Models.DataBase;
using System.Text;
using System.Text.Json;

namespace BatteryMonitorApp.PhysicalDeviceEmulator
{
    public class PhysicalDeviceEmulator
    {
        public double NominalCapacity { get; set; }
        public double NominalVolts { get; set; }
        


        public static readonly Guid Id = new ("DE88CE88-E888-8A88-8888-888888888888");
        public static readonly BatteryData TestBatteryData = new()
        {
            DeviceId = Id,
            Voltage = 12,
            DateTime = new DateTime(2000, 1, 1, 0, 0, 0, 0, 0),
            Status = 0,
            Current = 1,
            VoltageCharger = 0,
        };

        public static async Task<DateTime> DischargeApi(PhysicalDevice device, string urisite, CancellationToken token = default)
        {
            using var client = new HttpClient() { BaseAddress = new Uri(urisite) };
            double stepcapacity = 0;
            double capacity = device.NominalCapacity;
            long i = 0;

            HttpResponseMessage res;
            do
            {
                var currentVolts = GetVoltsIndex(capacity / device.NominalCapacity) * device.NominalVolts;
                stepcapacity = device.Current * device.Delaysecs / 3600;

                BatteryDataShortFormat data = new()
                {
                    C = (float)device.Current,
                    Di = device.DeviceId,
                    V = (float)currentVolts,
                    Dt = device.Start,
                    S = 3
                };
                try
                {
                    res = await PutDataAsync(client, data, token);
                }
                catch  { break; }
                device.Start = device.Start.AddSeconds(device.Delaysecs);
                capacity -= stepcapacity;
                i++;
            } while (!token.IsCancellationRequested && capacity > 0 && res.IsSuccessStatusCode);
            return device.Start;
        }

        public static async Task<HttpResponseMessage> PutDataAsync(HttpClient client, BatteryDataShortFormat data, CancellationToken token = default)
        {
            string json = JsonSerializer.Serialize(data);
            return await client.PostAsync("api/data", new StringContent(json, Encoding.UTF8, "application/json"), token);
        }


        internal static List<CapVolts> Arr = new()
            {
                new() {C=1, R=1.15 },
                new() {C=0.98, R=1.1 },new() {C= 0.96,R= 1.05 }, new(){C= 0.94,R= 1.02 },
                new() { C=0.92,R= 1.01 },new() {C= 0.91, R=1 },new(){C= 0.90,R= 0.99 },new() {C= 0.7,R= 0.9 },
                new() {C= 0.5, R=0.85 },new() {C= 0.3,R= 0.75 }, new() {C= 0.15,R= .65 }, new(){C=0.1,R= .35 },
                new() { C=0.09,R= 0.2}, new(){C= 0.07, R=0.1 }, new() {C= 0.0,R=0.00 }
            };

        public static double GetVoltsIndex(double capacityindex)
        {
            CapVolts res=new();
            CapVolts temp = Arr[0];
            foreach(var item in Arr)
            {
                if (item.C <= capacityindex)
                {
                    res =new() {R=item.R,C=item.C };
                    if (res.C == 1) break; 
                    var delta=(capacityindex-item.C)/(temp.C - item.C) *(temp.R-item.R);
                    res.R =item.R+delta;
                    break;
                }
                temp = item;
            }
            return res.R ;
        }
    }

    internal record CapVolts
    {
        public double C { get; set; }
        public double R { get; set; } = -1;
    }

}