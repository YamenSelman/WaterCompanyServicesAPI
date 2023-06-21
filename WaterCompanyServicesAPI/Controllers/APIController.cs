using Microsoft.AspNetCore.Mvc;
using ModelLibrary;

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
            _context.Database.EnsureCreated();
            if (_context.Departments.Count() <= 0)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        int barcode = 1;
                        string[] deps = new string[] { "DocumentControllerDep", "ConsumersDep", "GeneralManager", "SubCentersDep", "RepairingDep" };
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
                            user.UserName = $"{firstname.ToLower()}{i + 1}";
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
                                    sub.ConsumerSubscriptionNo = rnd.Next(1, 777777).ToString("D6");
                                }
                                while (subnos.Contains(sub.ConsumerSubscriptionNo));
                                subnos.Add(sub.ConsumerSubscriptionNo);
                                sub.SubscriptionAddress = areas[rnd.Next(areas.Length)];
                                sub.SubscriptionStatus = substate[rnd.Next(substate.Length)];
                                sub.SubscriptionUsingType = subtype[rnd.Next(subtype.Length)];
                                sub.RegisterDate = RandomDate(rnd);
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
                            sub.RegisterDate = RandomDate(rnd);
                            _context.Subscriptions.Add(sub);
                        }

                        _context.SaveChanges();

                        List<KeyValuePair<int, int>> cyc = new List<KeyValuePair<int, int>>();
                        cyc.Add(new KeyValuePair<int, int>(2022, 1));
                        cyc.Add(new KeyValuePair<int, int>(2022, 2));
                        cyc.Add(new KeyValuePair<int, int>(2022, 3));
                        cyc.Add(new KeyValuePair<int, int>(2022, 4));
                        cyc.Add(new KeyValuePair<int, int>(2022, 5));
                        cyc.Add(new KeyValuePair<int, int>(2022, 6));
                        cyc.Add(new KeyValuePair<int, int>(2023, 1));
                        cyc.Add(new KeyValuePair<int, int>(2024, 2));

                        foreach (var cons in consumers)
                        {
                            bool paid;
                            bool goodperson = rnd.Next(100) < 25;

                            foreach (var sub in cons.Subscriptions)
                            {
                                paid = true;
                                foreach (var cy in cyc)
                                {
                                    Invoice invoice = new Invoice();
                                    invoice.InvoiceCycle = cy.Value;
                                    invoice.InvoiceYear = cy.Key;
                                    if (sub.SubscriptionUsingType.Equals("residential"))
                                    {
                                        invoice.InvoiceValue = rnd.Next(500, 2000);
                                    }
                                    else
                                    {
                                        invoice.InvoiceValue = rnd.Next(5000, 50000);
                                    }
                                    invoice.InvoiceStatus = goodperson || paid;
                                    invoice.Subscription = sub;
                                    _context.Invoices.Add(invoice);
                                    if (cy.Value == rnd.Next(7) && paid)
                                    {
                                        paid = false;
                                    }
                                }
                            }
                        }

                        var subscriptions = _context.Subscriptions.Where(s => s.Consumer == null).ToList();
                        foreach (var sub in subscriptions)
                        {
                            foreach (var cy in cyc)
                            {
                                Invoice invoice = new Invoice();
                                invoice.InvoiceCycle = cy.Value;
                                invoice.InvoiceYear = cy.Key;
                                if (sub.SubscriptionUsingType.Equals("residential"))
                                {
                                    invoice.InvoiceValue = rnd.Next(500, 2000);
                                }
                                else
                                {
                                    invoice.InvoiceValue = rnd.Next(5000, 50000);
                                }
                                invoice.InvoiceStatus = false;
                                invoice.Subscription = sub;
                                _context.Invoices.Add(invoice);
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
            else
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        int barcode = Int32.Parse(_context.Subscriptions.Max(s=>s.ConsumerBarCode).ToString())+1;
                        string[] deps = new string[] { "DocumentControllerDep", "ConsumersDep", "GeneralManager", "SubCentersDep", "RepairingDep" };
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



                        int ii = _context.Consumers.Max(c => c.Id) + 1;
                        List<Consumer> newConsumers = new List<Consumer>();
                        for (int i = ii; i <= ii+c; i++)
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
                            user.UserName = $"{firstname.ToLower()}{i + 1}";
                            user.Password = firstname.ToLower();
                            user.UserType = "consumer";
                            user.AccountActive = true;
                            consumer.User = user;
                            _context.Consumers.Add(consumer);
                            _context.SaveChanges();
                            newConsumers.Add(consumer);
                        }
                        var consumers = _context.Consumers.ToList();
                        List<Subscription> newSubs = new List<Subscription>();
                        List<string> subnos = new List<string>();
                        foreach (Consumer cons in newConsumers)
                        {
                            int ct = rnd.Next(1, 4);
                            for (int j = 1; j <= ct; j++)
                            {
                                Subscription sub = new Subscription();
                                sub.ConsumerBarCode = barcode.ToString("D6");
                                barcode++;
                                do
                                {
                                    sub.ConsumerSubscriptionNo = rnd.Next(1, 777777).ToString("D6");
                                }
                                while (subnos.Contains(sub.ConsumerSubscriptionNo));
                                subnos.Add(sub.ConsumerSubscriptionNo);
                                sub.SubscriptionAddress = areas[rnd.Next(areas.Length)];
                                sub.SubscriptionStatus = substate[rnd.Next(substate.Length)];
                                sub.SubscriptionUsingType = subtype[rnd.Next(subtype.Length)];
                                sub.RegisterDate = RandomDate(rnd);
                                sub.Consumer = cons;
                                _context.Subscriptions.Add(sub);

                                newSubs.Add(sub);
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
                            sub.RegisterDate = RandomDate(rnd);
                            _context.Subscriptions.Add(sub);
                            
                            newSubs.Add(sub);
                        }

                        _context.SaveChanges();

                        List<KeyValuePair<int, int>> cyc = new List<KeyValuePair<int, int>>();
                        cyc.Add(new KeyValuePair<int, int>(2022, 1));
                        cyc.Add(new KeyValuePair<int, int>(2022, 2));
                        cyc.Add(new KeyValuePair<int, int>(2022, 3));
                        cyc.Add(new KeyValuePair<int, int>(2022, 4));
                        cyc.Add(new KeyValuePair<int, int>(2022, 5));
                        cyc.Add(new KeyValuePair<int, int>(2022, 6));
                        cyc.Add(new KeyValuePair<int, int>(2023, 1));
                        cyc.Add(new KeyValuePair<int, int>(2024, 2));

                        foreach (var cons in newConsumers)
                        {
                            bool paid;
                            bool goodperson = rnd.Next(100) < 25;

                            foreach (var sub in cons.Subscriptions)
                            {
                                paid = true;
                                foreach (var cy in cyc)
                                {
                                    Invoice invoice = new Invoice();
                                    invoice.InvoiceCycle = cy.Value;
                                    invoice.InvoiceYear = cy.Key;
                                    if (sub.SubscriptionUsingType.Equals("residential"))
                                    {
                                        invoice.InvoiceValue = rnd.Next(500, 2000);
                                    }
                                    else
                                    {
                                        invoice.InvoiceValue = rnd.Next(5000, 50000);
                                    }
                                    invoice.InvoiceStatus = goodperson || paid;
                                    invoice.Subscription = sub;
                                    _context.Invoices.Add(invoice);
                                    if (cy.Value == rnd.Next(7) && paid)
                                    {
                                        paid = false;
                                    }
                                }
                            }
                        }

                        var subscriptions = _context.Subscriptions.Where(s => s.Consumer == null).ToList();
                        foreach (var sub in subscriptions)
                        {
                            foreach (var cy in cyc)
                            {
                                Invoice invoice = new Invoice();
                                invoice.InvoiceCycle = cy.Value;
                                invoice.InvoiceYear = cy.Key;
                                if (sub.SubscriptionUsingType.Equals("residential"))
                                {
                                    invoice.InvoiceValue = rnd.Next(500, 2000);
                                }
                                else
                                {
                                    invoice.InvoiceValue = rnd.Next(5000, 50000);
                                }
                                invoice.InvoiceStatus = false;
                                invoice.Subscription = sub;
                                _context.Invoices.Add(invoice);
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

        public DateTime RandomDate(Random rnd)
        {
            int y, m, d =1;
            y = rnd.Next(2012, 2023);
            m = rnd.Next(1, 13);
            switch (m)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    d = rnd.Next(1, 32);
                    break;
                case 2:
                    d = rnd.Next(1, 29);
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    d = rnd.Next(1, 31);
                    break;
            }
            return new DateTime(y, m, d);
        }
    }
}
