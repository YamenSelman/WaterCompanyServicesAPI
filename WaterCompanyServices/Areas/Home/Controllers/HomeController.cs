using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using WaterCompanyModelLibrary;
using WaterCompanyServices.Areas.Consumer.Models;
using WaterCompanyServices.Areas.Home.DataAccess;
using WaterCompanyServices.Areas.Home.Models;
using WaterCompanyServices.Data;

namespace WaterCompanyServices.Areas.Home.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Home()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                var obj = HomeDA.Login("","");
                if (obj != null)
                {
                    String JsonObj = JsonConvert.SerializeObject(obj);

                    HttpContext.Session.SetString("User", JsonObj);

                    return RedirectToAction("UserDashBoard");
                }
            }
            return View(user);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}