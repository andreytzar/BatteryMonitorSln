
using AutoMapper;

using BatteryMonitorApp.Contracts.Models.Http;
using BatteryMonitorApp.Domain.Models.DataBase;

namespace BatteryMonitorApp.WebApp.MapperConfigs
{
    public class MapperConfigProfiles : Profile
    {
        public MapperConfigProfiles()
        {
            CreateMap<BatteryData, BatteryDataView>();
            CreateMap<BatteryDataShortFormat, BatteryData>().
                    ForMember(d => d.DeviceId, s => s.MapFrom(t => t.Di)).
                    ForMember(d => d.Voltage, s => s.MapFrom(t => t.V)).
                    ForMember(d => d.Current, s => s.MapFrom(t => t.C)).
                    ForMember(d => d.VoltageCharger, s => s.MapFrom(t => t.VC)).
                    ForMember(d => d.Status, s => s.MapFrom(t => t.S)).
                    ForMember(d => d.DateTime, s => s.MapFrom(t => t.Dt));
        }
    }
}
