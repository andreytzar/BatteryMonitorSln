using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BatteryMonitorApp.Contracts.Models.Http;
using BatteryMonitorApp.Domain.Repositories;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BatteryMonitorApp.WebApp.Controllers
{
    [Authorize]
    public class Physical : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository _repository;

        public Physical( UserManager<IdentityUser> userManager, IRepository repository)
        {
            _userManager = userManager;
            _repository = repository;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EmulatorAsync(CancellationToken token = default)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) { return Unauthorized(); }
            PhysicalDevice data = new()
            {
                Devices = (await _repository.GetRegisteredDevices(new Guid(user.Id), token)).Select(x =>
            new NameGuidDevice() { Name = x.DeviceName, Id = x.Id }).ToList()
            };
            return View(data);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EmulatorAsync(PhysicalDevice device, CancellationToken token = default)
        {
            var dev = device;
            var start = dev.Start;
            //http://user21507.realhost-free.net/
            string site = @$"http://{Request.Host.Value}";
            //string site = @$"http://user21507.realhost-free.net/";
            var end = await PhysicalDeviceEmulator.PhysicalDeviceEmulator.DischargeApi(dev, site, token);
            var date = new ReportGet()
            {
                DeviceId = dev.DeviceId,
                From = start,
                To = end
            };


            return RedirectToAction("Report", "report", date);
        }
    }
}
