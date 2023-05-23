using BatteryMonitorApp.Domain.Models.DataBase;
using BatteryMonitorApp.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace BatteryMonitorApp.UnitTests
{
    public class BatteryMonitorRepoTest
    {
        private readonly ILogger<IRepository> _logger;
        //private readonly IBatteryMonitorContext _context ;
        public BatteryMonitorRepoTest()
        {
            _logger = new Mock<ILogger<IRepository>>().Object;
            
        }
        [Fact]
        public void RepoIsCreated()
        {
            // Arrange
            using var _context = Helper.CreateMemoryContext();
             var repo =new Repository(_context,_logger);
            // Act
            // Assert
            Assert.NotNull(repo);
        }
        [Fact]
        public async Task RepoAddData_CanAdd_RusultTrue_AndCorrect()
        {
            // Arrange
            using var _context = Helper.CreateMemoryContext();
             var repo = new Repository(_context, _logger);
            var data = PhysicalDeviceEmulator.PhysicalDeviceEmulator.TestBatteryData;
            // Act
            int res = await repo.AddData(data);
            // Assert
            Assert.True(res > 0);
            Assert.Contains(_context.BatteryDatas, x => x == data);
        }

        [Fact]
        public async Task RepoGetBatteryData_DataNotEmpty_AndCorrect()
        {
            // Arrange
            using var _context = Helper.CreateMemoryContext();
            var repo = new Repository(_context, _logger);
            var data =new BatteryData();
            var devid=Guid.NewGuid();
            var dev = new BatteryRegisteredDevice() { Id = devid, DeviceName = "Test", UserId = Guid.NewGuid() };
            _context.Devices.Add(dev);
            await _context.SaveChangesAsync();
            data.DeviceId= devid;
            _ = await repo.AddData(data);
            // Act
            var res=await repo.GetBatteryData(data.DeviceId,data.DateTime.AddDays(-1),
                data.DateTime.AddDays(1), new int[] {data.Status });
            //Assert
            Assert.True(res.Length > 0);
            Assert.Contains(res, x => x == data);
        }
        [Fact]
        public async Task AddRegisteredDevices_DeviceAdded_AndCorrect()
        {
            // Arrange
            using var _context = Helper.CreateMemoryContext();
             var repo = new Repository(_context, _logger);
            // Act
            var dev = new BatteryRegisteredDevice() {DeviceName = "Test", UserId = Guid.NewGuid() };
            var res=await repo.AddRegisteredDevices(dev);
            //Assert
            Assert.True(res > 0);
            Assert.Contains(_context.Devices, x => x == dev);
        }

        [Fact]
        public async Task GetRegisteredDevices_NotNull_AndArrayCorrect()
        {
            // Arrange
            using var _context = Helper.CreateMemoryContext();
             var repo = new Repository(_context, _logger);
            var usredId =Guid.NewGuid();
            var dev = new BatteryRegisteredDevice() { UserId = usredId, DeviceName = "Test" };
            var dev1 = new BatteryRegisteredDevice() { DeviceName = "Test", UserId = usredId };
            var dev2 = new BatteryRegisteredDevice() { UserId = usredId, DeviceName = "Test3" };
            await repo.AddRegisteredDevices(dev);
            await repo.AddRegisteredDevices(dev1);
            await repo.AddRegisteredDevices(dev2);
            // Act
            var res=await repo.GetRegisteredDevices(usredId);
            //Assert
            Assert.NotNull(res);
            Assert.NotEmpty(res);
            Assert.True(res.Length == 3);
            Assert.Contains(res, x=> x == dev);
        }


        [Fact]
        public async Task DeviseIsRegistered()
        {
            // Arrange
            using var _context = Helper.CreateMemoryContext();
             var repo = new Repository(_context, _logger);
            var usredId = Guid.NewGuid();
            var dev = new BatteryRegisteredDevice() { UserId = usredId, DeviceName = "Test" };
            var dev1 = new BatteryRegisteredDevice() { DeviceName = "Test", UserId = usredId };
            var dev2 = new BatteryRegisteredDevice() { UserId = usredId, DeviceName = "Test3" };
            await repo.AddRegisteredDevices(dev);
            await repo.AddRegisteredDevices(dev1);
            // Act
            var res = await repo.DeviseIsRegistered(dev.Id);
            var res1 = await repo.DeviseIsRegistered(dev1.Id);
            var res2 = await repo.DeviseIsRegistered(dev2.Id);
            //Assert
            Assert.True(res);
            Assert.True(res1);
            Assert.True(!res2);
        }

    }
}
