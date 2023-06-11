using Microsoft.AspNetCore.Mvc;
using ModelLibrary;
using WaterCompanyServiceWebSite.Models;

namespace WaterCompanyServiceWebSite.Controllers
{
    public class EmployeePanelController : Controller
    {
        public IActionResult Index()
        {
            List<Request> pendingRequests = DataAccess.GetPendingRequests();
            return View(pendingRequests);
        }

        public IActionResult ViewRequest(int id)
        {
            ViewRequestObj obj = new ViewRequestObj();
            obj.Request = DataAccess.GetRequest(id);
            obj.Log = new RequestsLog();

            if (obj.Request != null)
            {
                switch(obj.Request.RequestType)
                {
                    case "attach":
                        return View("ViewAttachRequest",obj);
                    case "clearance":
                        var invoices = DataAccess.GetInvoices(obj.Request.Subscription.ConsumerBarCode);
                        if(invoices != null)
                        {
                            ViewData["total"] = invoices.Sum(i=>i.InvoiceValue);
                            ViewData["unpaid"] = invoices.Where(i=>i.InvoiceStatus == false).Sum(i=>i.InvoiceValue);
                        }
                        else
                        {
                            ViewData["total"] = "No Invoices";
                            ViewData["unpaid"] = "No Invoices";
                        }
                        return View("ViewClearanceRequest", obj);
                    case "new":
                        return View("ViewNewSubscriptionRequest", obj);
                    default:
                        return RedirectToAction("Index");
                }
            }
            else
            {
                List<Request> pendingRequests = DataAccess.GetPendingRequests();
                return View("Index",pendingRequests);
            }
        }

        public IActionResult Logout()
        {
            DataAccess.CurrentUser = null;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult ProcessRequest(ViewRequestObj obj)
        {
            if(obj.Request != null)
            {
                if(obj.Log.Decision)
                {
                    if (DataAccess.AcceptRequest(obj.Request.Id, obj.Log.Notes))
                    {
                        ViewData["msg"] = "Request Processed Success";
                    }
                    else
                    {
                        ViewData["msg"] = "Failed To Process Request";
                    }
                }
                else
                {
                    if(DataAccess.RejectRequest(obj.Request.Id,obj.Log.Notes))
                    {
                        ViewData["msg"] = "Request Processed Success";
                    }
                    else
                    {
                        ViewData["msg"] = "Failed To Process Request";
                    }
                }
            }
            else
            {
                ViewData["msg"] = "Request Not Found";
            }
            return RedirectToAction("index");
        }
    }
}
