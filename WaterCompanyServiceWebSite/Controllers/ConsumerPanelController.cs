using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ModelLibrary;
using WaterCompanyServiceWebSite.Models;

namespace WaterCompanyServiceWebSite.Controllers
{
    public class ConsumerPanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logout()
        {
            DataAccess.CurrentUser = null;
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AddSubscription()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSubscription(Subscription sub)
        {
            sub = DataAccess.GetSubscriptionByBarcode(sub.ConsumerBarCode);
            if (sub == null)
            {
                ViewBag.Message = "No subscription with this barcode";
            }
            else if (sub.Consumer != null)
            {
                ViewBag.Message = "This subscription is attached to another consumer";
                return View(null);
            }
            return View(sub);
        }

        public IActionResult SubmitAttachRequest(Subscription sub)
        {
            try
            {
                sub = DataAccess.GetSubscriptionByBarcode(sub.ConsumerBarCode);
                if (sub != null)
                {
                    if (sub.Consumer == null)
                    {
                        Request req = new Request();
                        req.RequestType = "attach";
                        req.CurrentDepartment = DataAccess.GetDepartments().Where(d => d.Id == 2).FirstOrDefault();
                        req.RequestDate = DateTime.Now;
                        req.Consumer = DataAccess.GetCurrentConsumer();
                        req.Subscription = sub;
                        req.RequestStatus = "onprogress";
                        req = DataAccess.AddRequest(req);
                        if (req != null)
                        {
                            ViewBag.Message = "Request added successfully";
                        }
                        else
                        {
                            ViewBag.Message = "Error submitting the request";
                        }

                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.Message = $"Error: {e.Message}";
            }
            return View("Index");
        }


        public IActionResult ClearanceRequest()
        {
            var subs = DataAccess.GetConsumerSubscription();
            return View(subs);
        }

        public IActionResult SubmitClearanceRequest(int sid)
        {
            var sub = DataAccess.GetSubscription(sid);
            if (sub != null)
            {
                try
                {
                    var unpaidInvoices = DataAccess.GetUnpaidInvoices(sub.ConsumerBarCode).Sum(i => i.InvoiceValue);
                    if (unpaidInvoices <= 0)
                    {
                        Request req = new Request();
                        req.RequestType = "clearance";
                        req.CurrentDepartment = DataAccess.GetDepartments().Where(d => d.Id == 2).FirstOrDefault();
                        req.RequestDate = DateTime.Now;
                        req.Consumer = DataAccess.GetCurrentConsumer();
                        req.Subscription = sub;
                        req.RequestStatus = "onprogress";
                        req = DataAccess.AddRequest(req);
                        if (req != null)
                        {
                            ViewBag.Message = "Request added successfully";
                        }
                        else
                        {
                            ViewBag.Message = "Error submiting the request";
                        }

                    }
                    else
                    {
                        ViewBag.Message = $"The subscription have unpaid invoices .. Total unpaid amount = {unpaidInvoices}";
                        var subs = DataAccess.GetConsumerSubscription();
                        return View("ClearanceRequest", subs);
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Message = $"Error: {e.Message}";
                }
                return View("index");
            }
            else
            {
                var subs = DataAccess.GetConsumerSubscription();
                return View("ClearanceRequest", subs);
            }

        }

        public IActionResult NewSubscription()
        {
            return View();
        }


        [HttpPost]
        public IActionResult SubmitNewSubscriptionRequest(NewSubscriptionVM obj)
        {
            byte[] DocumentBA = null;
            if (ModelState.IsValid)
            {
                if (obj.Document.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        obj.Document.CopyTo(stream);
                        DocumentBA = stream.ToArray();

                        try
                        {
                            Request req = new Request();
                            req.RequestType = "new";
                            req.CurrentDepartment = DataAccess.GetDepartments().Where(d => d.Id == 1).FirstOrDefault();
                            req.RequestDate = DateTime.Now;
                            req.Consumer = DataAccess.GetCurrentConsumer();
                            req.RequestStatus = "onprogress";
                            req.Details = new RequestDetails();
                            req.Details.NewSubAddress = obj.Address;
                            req.Details.NewSubType = obj.UsingType;
                            req.Details.Document = DocumentBA;
                            req = DataAccess.AddRequest(req);
                            if (req != null)
                            {
                                ViewData["msg"] = "Request added successfully";
                            }
                            else
                            {
                                ViewData["msg"] = "Error submiting the request";
                            }
                            return View("Index");
                        }
                        catch (Exception e)
                        {
                            ViewData["msg"] = $"Error: {e.Message}";
                            return View("NewSubscription", obj);
                        }
                    }
                }
                ViewData["msg"] = "Document not valid";
                return View("NewSubscription", obj);
            }
            else
            {
                ViewData["msg"] = "Request form not valid";
                return View("NewSubscription", obj);
            }
        }
    }
}
