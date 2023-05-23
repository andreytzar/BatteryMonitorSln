using BatteryMonitorApp.Domain.Models.DataBase;
using Microsoft.EntityFrameworkCore;

namespace BatteryMonitorApp.UnitTests
{
     public class BatteryMonitorContextTest
    {

        [Fact]
#pragma warning disable CS1998 
        public async Task CreateContextTestAndDbsetsNotEmpty()
#pragma warning restore CS1998 
        {
            var options = new DbContextOptionsBuilder<MemoryContext>()
            .UseInMemoryDatabase(databaseName: "ListDatabase")
            .Options;
            var con =new MemoryContext(options);
            var dev1 = new BatteryRegisteredDevice() { Id = PhysicalDeviceEmulator.PhysicalDeviceEmulator.Id, DeviceName = "Test", UserId = Guid.NewGuid() };
            var dev2 = new BatteryRegisteredDevice() { Id = Guid.NewGuid(), DeviceName = "Test", UserId = Guid.NewGuid() };
            var dev3 = new BatteryRegisteredDevice() { Id = Guid.NewGuid(), DeviceName = "Test", UserId = Guid.NewGuid() };
            var data = new BatteryData() { Device = dev1 };
            var data1 = new BatteryData() { Device = dev1 };
            var data2 = new BatteryData() { Device = dev1 };
            con.Devices.Add(dev1);
            con.Devices.Add(dev2);
            con.Devices.Add(dev3);
            con.BatteryDatas.Add(data);
            con.BatteryDatas.Add(data1);
            con.BatteryDatas.Add(data2);
            con.SaveChanges();
            //Asserts
            Assert.Contains(con.Devices, x => x == dev1);
            Assert.Contains(con.BatteryDatas, x => x == data);
        }
    }
}
