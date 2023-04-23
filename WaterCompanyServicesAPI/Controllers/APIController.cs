using Microsoft.AspNetCore.Mvc;

namespace WaterCompanyServicesAPI.Controllers
{
    [Route("API")]
    public class APIController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public Boolean GetApi()
        {
            return true;
        }
    }
}
