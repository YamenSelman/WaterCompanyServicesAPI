using Microsoft.AspNetCore.Mvc;
using WaterCompanyServicesAPI.Models;

namespace WaterCompanyServicesAPI.Controllers
{
    [Route("API")]
    public class APIController : Controller
    {
        private readonly ILogger<APIController> _logger;
        private readonly WaterCompanyDBContext _context;
        Random rnd = new Random();
        public APIController(ILogger<APIController> logger, WaterCompanyDBContext _context)
        {
            this._logger = logger;
            this._context = _context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public Boolean GetApi()
        {
            return true;
        }

        [Route("seed/{c}")]
        [HttpGet]
        public bool Seed(int c)
        {
            if (_context.Departments.Count() <= 0)
            {
                using(var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        int barcode = 1;
                        string[] deps = new string[] { "ConsumerDP", "GeneralManager", "MaintainanceDP", "FinanceDP", "DocumnerControllerDP" };
                        string[] mname = new string[] { "Mohammad", "Ahmad", "Basel", "Ali", "Omar", "Samer", "Ibrahem", "Abdalla", "Somar", "Badr" };
                        string[] fname = new string[] { "Samira", "Ola", "Nisreen", "Salwa", "Rima", "Sawsan", "Hala", "Hadeel", "Nagham", "Mona" };
                        string[] sname = new string[] { "Mohammad", "Ahmad", "Ali", "Omar", "Samer", "Ibrahem", "Abdalla", "Somar", "Badr", "Mahmoud" };
                        string[] areas = new string[] { "Area1", "Area2", "Area3", "Area4", "Area5", "Area6", "Area7", "Area8", "Area9" };
                        string[] substate = new string[] { "active", "active", "active", "active", "inactive" };
                        string[] subtype = new string[] { "residential", "residential", "residential", "residential", "commercial", "industrial" };
                        List<string[]> gendername = new List<string[]>();
                        gendername.Add(mname);
                        gendername.Add(fname);
                        List<string> gender = new List<string>();
                        gender.Add("male");
                        gender.Add("female");

                        User admin = new User();
                        admin.UserName = "admin";
                        admin.Password = "admin";
                        admin.UserType = "admin";
                        admin.AccountActive = true;
                        _context.Users.Add(admin);

                        foreach (string dep in deps)
                        {
                            Department d = new Department();
                            d.DepartmentName = dep;
                            _context.Departments.Add(d);
                        }

                        for (int i = 1; i <= c; i++)
                        {
                            System.Diagnostics.Debug.WriteLine($"*****( {i} )*****");

                            Consumer consumer = new Consumer();
                            consumer.ConsumerAddress = areas[rnd.Next(areas.Length)];
                            int g = rnd.Next(2);
                            consumer.ConsumerGender = gender[g];
                            string firstname = gendername[g][rnd.Next(gendername[g].Length)];
                            string lastname = sname[rnd.Next(sname.Length)];
                            consumer.ConsumerName = $"{firstname} {lastname}";
                            consumer.ConsumerPhone = "";
                            for (int j = 0; j < 10; j++)
                            {
                                consumer.ConsumerPhone += rnd.Next(9).ToString();
                            }
                            consumer.ConsumerAge = rnd.Next(18, 60);
                            User user = new User();
                            user.UserName = $"{firstname}_{i+1}";
                            user.Password = firstname.ToLower();
                            user.UserType = "consumer";
                            user.AccountActive = true;
                            consumer.User = user;
                            _context.Consumers.Add(consumer);
                            _context.SaveChanges();
                        }
                        var consumers = _context.Consumers.ToList();
                        List<string> subnos = new List<string>();
                        foreach (Consumer cons in consumers)
                        {
                            int ct = rnd.Next(1, 4);
                            for (int j = 1; j <= ct; j++)
                            {
                                Subscription sub = new Subscription();
                                sub.ConsumerBarCode = barcode.ToString("D6");
                                barcode++;
                                do
                                {
                                    sub.ConsumerSubscriptionNo = rnd.Next(1, 999999).ToString("D6");
                                }
                                while (subnos.Contains(sub.ConsumerSubscriptionNo));
                                subnos.Add(sub.ConsumerSubscriptionNo);
                                sub.SubscriptionAddress = areas[rnd.Next(areas.Length)];
                                sub.SubscriptionStatus = substate[rnd.Next(substate.Length)];
                                sub.SubscriptionUsingType = subtype[rnd.Next(subtype.Length)];
                                sub.Consumer = cons;
                                _context.Subscriptions.Add(sub);
                            }
                        }

                        for (int j = 1; j <= c; j++)
                        {
                            Subscription sub = new Subscription();
                            sub.ConsumerBarCode = barcode.ToString("D6");
                            barcode++;
                            do
                            {
                                sub.ConsumerSubscriptionNo = rnd.Next(1, 999999).ToString("D6");
                            }
                            while (subnos.Contains(sub.ConsumerSubscriptionNo));
                            subnos.Add(sub.ConsumerSubscriptionNo);
                            sub.SubscriptionAddress = areas[rnd.Next(areas.Length)];
                            sub.SubscriptionStatus = substate[rnd.Next(substate.Length)];
                            sub.SubscriptionUsingType = subtype[rnd.Next(subtype.Length)];
                            _context.Subscriptions.Add(sub);
                        }

                        _context.SaveChanges();

                        foreach (var cons in consumers)
                        {
                            foreach (var sub in cons.Subscriptions)
                            {
                                Bill bill = new Bill();
                                bill.Number = "01";
                                bill.Year = "2022";
                                bill.Amount = 1500;
                                bill.Status = true;
                                bill.Subscription = sub;
                                _context.Bills.Add(bill);
                            }
                        }

                        _context.SaveChanges();

                        transaction.Commit();

                        System.Diagnostics.Debug.WriteLine($"|**************|");
                        System.Diagnostics.Debug.WriteLine($"|***( Done )***|");
                        System.Diagnostics.Debug.WriteLine($"|**************|");

                        return true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                        transaction.Rollback();
                    }
                }
            }
            return false;
        }
    }
}
