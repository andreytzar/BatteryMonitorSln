using BatteryMonitorApp.Domain.DbContexts;
using BatteryMonitorApp.Domain.Models.DataBase;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace BatteryMonitorApp.UnitTests
{
    public class Helper
    {
        public static IBatteryMonitorContext CreateMemoryContext()
        {
            var options = new DbContextOptionsBuilder<MemoryContext>()
           .UseInMemoryDatabase(databaseName: "MovieListDatabase")
           .Options;
            return new MemoryContext(options);
        }
    }

    public class MemoryContext : DbContext, IBatteryMonitorContext
    {

        public DbSet<BatteryData> BatteryDatas { get; set; }
        public DbSet<BatteryRegisteredDevice> Devices { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
        public override ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default)
        {
            return base.AddAsync(entity, cancellationToken);
        }
        public MemoryContext(DbContextOptions options) : base(options)
        {
        }
    }
}
