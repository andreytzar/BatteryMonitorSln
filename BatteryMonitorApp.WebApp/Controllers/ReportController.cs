using BatteryMonitorApp.Contracts.Models.Http;
using BatteryMonitorApp.Domain.Repositories;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BatteryMonitorApp.Contracts.Models;
using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace BatteryMonitorApp.WebApp.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository _repository;

        public ReportController( UserManager<IdentityUser> userManager, IRepository repository)
        {
            _userManager = userManager;
            _repository = repository;
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Report(ReportGet data = null, CancellationToken token = default)
        {
            if (data == null) data = new();
            var user = await _userManager.GetUserAsync(User);
            if (user == null) { return Unauthorized(); }
            if (data.DeviceId != Guid.Empty)
            {
                var devices = (await _repository.GetRegisteredDevices(new Guid(user.Id), default)).Select(x =>
                new NameGuidDevice() { Name = x.DeviceName, Id = x.Id }).ToList();
                if (!devices.Any(x => x.Id == data.DeviceId)) return BadRequest();
                data.Devices = devices;
                var battdata = (await _repository.GetBatteryData(data.DeviceId, data.From, data.To,
                    new[] { 0, 1, 2, 3, 4, 5, 6 }, token)).
                    Select(x => new BatteryDataView()
                    {
                        C = x.Current,
                        DT = x.DateTime,
                        VC = x.VoltageCharger,
                        S = (BatteryEventStatus)x.Status,
                        V = x.Voltage
                    }
                    ).ToList();
                data.BatteryDataViews = battdata.Where(x => x != null).ToList();
                double capacity = 0;
                BatteryDataView previewitem = null;
                foreach (BatteryDataView item in battdata)
                {
                    if (previewitem == null)
                    {
                        previewitem = item;
                        continue;
                    }
                    capacity += (double)item.C / 3600 * ((DateTime)item.DT).Subtract((DateTime)previewitem.DT).TotalSeconds;
                    previewitem = item;
                }
                data.Capacity = capacity;
            }
            else
                data.Devices = (await _repository.GetRegisteredDevices(new Guid(user.Id), default)).Select(x =>
                new NameGuidDevice() { Name = x.DeviceName, Id = x.Id }).ToList();
            return View(data);
        }

    }
}
