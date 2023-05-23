using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BatteryMonitorApp.Contracts.Models.Http;
using BatteryMonitorApp.Domain.Models.DataBase;
using BatteryMonitorApp.Domain.Repositories;
using BatteryMonitorApp.WebApp.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BatteryMonitorApp.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository _repository;

        public HomeController( UserManager<IdentityUser> userManager, IRepository repository)
        {
            _userManager = userManager;
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult WikiApi()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> GetRegisteredDevices(CancellationToken token = default)
        {
            BatteryDevice[] result = Array.Empty<BatteryDevice>();
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                result = (await _repository.GetRegisteredDevices(new Guid(user.Id), token))
               .Select(x => new BatteryDevice()
               {
                   Id = x.Id,
                   DeviceDescription = x.DeviceDescription,
                   DeviceName = x.DeviceName
               }).ToArray();
            }
            return View(result);
        }
        [Authorize]
        public IActionResult CreateRegisteredDevices()
        {
            var data = new BatteryDevice();
            return View(data);
        }
        [HttpPost]
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateRegisteredDevices(BatteryDevice device, CancellationToken token = default)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Error");
            BatteryRegisteredDevice dev = new()
            {
                DeviceDescription = device.DeviceDescription,
                DeviceName = device.DeviceName,
                Id = device.Id,
                UserId = new Guid(user.Id)
            };
            await _repository.AddRegisteredDevices(dev, token);
            return RedirectToAction("GetRegisteredDevices");
        }

        public IActionResult Technology()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    }
}