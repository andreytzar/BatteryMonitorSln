using System;
using System.Threading;
using System.Threading.Tasks;

using BatteryMonitorApp.Domain.Models.DataBase;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BatteryMonitorApp.Domain.DbContexts
{
    public interface IBatteryMonitorContext:IDisposable
    {
        DbSet<BatteryData> BatteryDatas { get; set; }
        DbSet<BatteryRegisteredDevice> Devices { get; set; }
        Task<int> SaveChangesAsync(CancellationToken token=default);
        int SaveChanges();
        ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default);
    }

    public class BatteryMonitorContext : IdentityDbContext, IBatteryMonitorContext
    {
        virtual public DbSet<BatteryData> BatteryDatas { get; set; }
        virtual public DbSet<BatteryRegisteredDevice> Devices { get; set; }

        public BatteryMonitorContext() { }
        public BatteryMonitorContext(DbContextOptions options) : base(options)
        {
        }
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
    }
}
