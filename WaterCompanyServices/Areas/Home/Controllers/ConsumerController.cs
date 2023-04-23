using Microsoft.AspNetCore.Mvc;
using WaterCompanyModelLibrary;
using WaterCompanyServices.Areas.Home.DataAccess;

namespace WaterCompanyServices.Areas.Home.Controllers
{
    public class ConsumerController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<WaterCompanyModelLibrary.Consumer> ConsumerAccounts = await HomeDA.GetConsumerAccounts();
            return View(ConsumerAccounts);
        }
    }
}
