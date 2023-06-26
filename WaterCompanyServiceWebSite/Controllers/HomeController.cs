using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using ModelLibrary;
using WaterCompanyServiceWebSite.Models;
using System.Buffers.Text;
using System.Reflection;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;

namespace WaterCompanyServiceWebSite.Controllers
{
    public class HomeController : Controller
    {
        Random rnd = new Random();
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            DataAccess.DoForecast();
            return View();

        }

        public IActionResult Login()
        {
            if(DataAccess.CurrentUser != null)
            {
                return RedirectUser();
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            User loginUser = DataAccess.Login(user);
            if (loginUser == null)
            {
                ViewData["message"] = "Username or password incorrect";
                return View();
            }
            else if (!loginUser.AccountActive)
            {
                ViewData["message"] = "This account is not active";
                return View();
            }
            else
            {
                DataAccess.CurrentUser = loginUser;
                return RedirectUser();
            }
        }

        public IActionResult Logout()
        {
            DataAccess.CurrentUser = null;
            return RedirectToAction("Index", "Home");
        }

        public IActionResult RedirectUser()
        {
            switch (DataAccess.CurrentUser.UserType)
            {
                case "admin":
                    return RedirectToAction("Index", "AdminPanel");
                case "employee":
                    return RedirectToAction("Index", "EmployeePanel");
                case "consumer":
                    return RedirectToAction("Index", "ConsumerPanel");
                default:
                    return View("Error");
            }
        }

        public IActionResult Register()
        {
            if (DataAccess.CurrentUser != null)
            {
                return RedirectUser();
            }
            Consumer consumer = new Consumer();
            consumer.Subscriptions = new List<Subscription>(); ;
            consumer.User = new User();
            consumer.User.UserType = "consumer";
            return View(consumer);
        }

        [HttpPost]
        public IActionResult Register(Consumer consumer,String passwordConfirm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                if(DataAccess.UserNameExists(consumer.User.UserName))
                {
                    ViewData["message"] = "User name already exists";
                    return View(consumer);
                }
                else if (consumer.User.Password != passwordConfirm)
                {
                    ViewData["message"] = "Password does not match";
                    return View(consumer);
                }
                else
                {
                    consumer.User.UserType = "consumer";
                    consumer.User.AccountActive = false;
                    DataAccess.AddConsumer(consumer);
                    ViewData["message"] = "Thanks for your registeration, Your account will be reviewed and activated as soon as possible";
                    return View("Index");
                }
            }
        }

        public IActionResult Privacy()
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