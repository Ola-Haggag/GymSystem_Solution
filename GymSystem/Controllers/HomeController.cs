using GymSystem.BLL.Services.Interfaces;
using GymSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GymSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnalyticsServices analyticsServices;

        public HomeController(ILogger<HomeController> logger, IAnalyticsServices analyticsServices)
        {
            _logger = logger;
            this.analyticsServices = analyticsServices;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Data = await analyticsServices.GetAnalyticsDataAsync(ct);
            return View(Data);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location =ResponseCacheLocation.None, NoStore =true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
