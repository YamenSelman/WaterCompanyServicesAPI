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
        public IActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]        
        public IActionResult ChangePassword(ChangePasswordVM vm)
        {
            if(vm.NewPassword == null || vm.OldPassword== null || vm.ConfirmPassword == null) {
                ViewData["message"] = "Please insert all the fields";
                return View();
            }
            if(vm.OldPassword != DataAccess.CurrentUser.Password)
            {
                ViewData["message"] = "Old password is incorrect";
                return View();
            }
            if(vm.NewPassword != vm.ConfirmPassword)
            {
                ViewData["message"] = "New password mismatch";
                return View();
            }
            DataAccess.CurrentUser.Password = vm.NewPassword;
            DataAccess.UpdateUser(DataAccess.CurrentUser);
            ViewData["message"] = "Password changed successfully";
            List<Request> pendingRequests = DataAccess.GetPendingRequests();
            return View("Index", pendingRequests);
        }

        public IActionResult ViewRequest(int id)
        {
            RequestProcessVM obj = new RequestProcessVM();
            obj.Request = DataAccess.GetRequest(id);
            obj.Log = new RequestsLog();

            if (obj.Request != null)
            {
                if(obj.Request.RequestType != "new")
                {
                    obj.Request.Subscription = DataAccess.GetSubscriptionByBarcode(obj.Request.Subscription.ConsumerBarCode);
                }

                switch (obj.Request.RequestType)
                {
                    case "attach":
                        return View("ViewAttachRequest", obj);
                    case "clearance":
                        var invoices = DataAccess.GetInvoices(obj.Request.Subscription.ConsumerBarCode);
                        if (invoices != null)
                        {
                            ViewData["total"] = invoices.Sum(i => i.InvoiceValue);
                            ViewData["unpaid"] = invoices.Where(i => i.InvoiceStatus == false).Sum(i => i.InvoiceValue);
                        }
                        else
                        {
                            ViewData["total"] = "No Invoices";
                            ViewData["unpaid"] = "No Invoices";
                        }
                        return View("ViewClearanceRequest", obj);
                    case "new":
                        return View("ViewNewSubscriptionRequest", obj);
                    case "repair":
                        ViewData["repairemp"] = DataAccess.GetCurrentEmployee().Department.Id == 5;
                        return View("ViewRepairRequest", obj);
                    case "transfer":
                        return View("ViewTransferRequest", obj);
                    default:
                        return RedirectToAction("Index");
                }
            }
            else
            {
                List<Request> pendingRequests = DataAccess.GetPendingRequests();
                return View("Index", pendingRequests);
            }
        }

        public IActionResult Logout()
        {
            DataAccess.CurrentUser = null;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult ProcessRequest(RequestProcessVM obj)
        {
            string msg = "";
            bool result;
            if (obj.Request != null)
            {
                if (obj.Log.Decision)
                {
                    result = DataAccess.AcceptRequest(obj.Request.Id, obj.Log.Notes);
                }
                else
                {
                    result = DataAccess.RejectRequest(obj.Request.Id, obj.Log.Notes);
                }
                if (result)
                {
                    msg = "Request Processed Success";
                }
                else
                {
                    msg = "Failed To Process Request";
                }
            }
            else
            {
                msg = "Request Not Found";
            }
            ViewData["message"] = msg;
            List<Request> pendingRequests = DataAccess.GetPendingRequests();
            return View("index",pendingRequests);
        }
    }
}
