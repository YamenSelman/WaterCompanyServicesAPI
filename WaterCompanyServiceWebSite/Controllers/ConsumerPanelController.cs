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

        public IActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordVM vm)
        {
            if (vm.NewPassword == null || vm.OldPassword == null || vm.ConfirmPassword == null)
            {
                ViewData["message"] = "Please insert all the fields";
                return View();
            }
            if (vm.OldPassword != DataAccess.CurrentUser.Password)
            {
                ViewData["message"] = "Old password is incorrect";
                return View();
            }
            if (vm.NewPassword != vm.ConfirmPassword)
            {
                ViewData["message"] = "New password mismatch";
                return View();
            }
            DataAccess.CurrentUser.Password = vm.NewPassword;
            DataAccess.UpdateUser(DataAccess.CurrentUser);
            ViewData["message"] = "Password changed successfully";
            return View("Index");
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
                ViewData["message"] = "No subscription with this barcode";
            }
            else if (sub.Consumer != null)
            {
                ViewData["message"] = "This subscription is attached to another consumer";
            }
            else
            {
                ViewData["message"] = null;
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
                            ViewData["message"] = "Request added successfully";
                        }
                        else
                        {
                            ViewData["message"] = "Error submitting the request";
                        }

                    }
                }
            }
            catch (Exception e)
            {
                ViewData["message"] = $"Error: {e.Message}";
            }
            return View("Index");
        }


        public IActionResult ClearanceRequest()
        {
            var subs = DataAccess.GetConsumerSubscription();
            ViewData["requestType"] = "clearance";
            return View("SubscriptionSelect", subs);
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
                            ViewData["message"] = "Request added successfully";
                        }
                        else
                        {
                            ViewData["message"] = "Error submiting the request";
                        }

                    }
                    else
                    {
                        ViewData["message"] = $"The subscription have unpaid invoices .. Total unpaid amount = {unpaidInvoices}";
                        var subs = DataAccess.GetConsumerSubscription();
                        ViewData["requestType"] = "clearance";
                        return View("SubscriptionSelect", subs);
                    }
                }
                catch (Exception e)
                {
                    ViewData["message"] = $"Error: {e.Message}";
                }
                return View("index");
            }
            else
            {
                var subs = DataAccess.GetConsumerSubscription();
                ViewData["requestType"] = "clearance";
                return View("SubscriptionSelect", subs);
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
                                ViewData["message"] = "Request added successfully";
                            }
                            else
                            {
                                ViewData["message"] = "Error submiting the request";
                            }
                            return View("Index");
                        }
                        catch (Exception e)
                        {
                            ViewData["message"] = $"Error: {e.Message}";
                            return View("NewSubscription", obj);
                        }
                    }
                }
                ViewData["message"] = "Document not valid";
                return View("NewSubscription", obj);
            }
            else
            {
                ViewData["message"] = "Request form not valid";
                return View("NewSubscription", obj);
            }
        }

        public IActionResult RequistMeterRepair()
        {
            var subs = DataAccess.GetConsumerSubscription();
            ViewData["requestType"] = "repair";
            return View("SubscriptionSelect", subs);
        }

        public IActionResult SubmitRepairRequest(int sid)
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
                        req.RequestType = "repair";
                        req.CurrentDepartment = DataAccess.GetDepartments().Where(d => d.Id == 2).FirstOrDefault();
                        req.RequestDate = DateTime.Now;
                        req.Consumer = DataAccess.GetCurrentConsumer();
                        req.Subscription = sub;
                        req.RequestStatus = "onprogress";
                        req = DataAccess.AddRequest(req);
                        if (req != null)
                        {
                            ViewData["message"] = "Request added successfully";
                        }
                        else
                        {
                            ViewData["message"] = "Error submiting the request";
                        }

                    }
                    else
                    {
                        ViewData["message"] = $"The subscription have unpaid invoices .. Total unpaid amount = {unpaidInvoices}";
                        var subs = DataAccess.GetConsumerSubscription();
                        ViewData["requestType"] = "clearance";
                        return View("SubscriptionSelect", subs);
                    }
                }
                catch (Exception e)
                {
                    ViewData["message"] = $"Error: {e.Message}";
                }
                return View("index");
            }
            else
            {
                var subs = DataAccess.GetConsumerSubscription();
                ViewData["requestType"] = "clearance";
                return View("SubscriptionSelect", subs);
            }

        }

        public IActionResult InvoiceRequest()
        {
            var subs = DataAccess.GetConsumerSubscription();
            ViewData["requestType"] = "invoice";
            return View("SubscriptionSelect", subs);
        }

        public IActionResult ViewSubscriptionInvoices(int sid)
        {
            InvoicesVM model = new InvoicesVM();
            model.Subscription = DataAccess.GetSubscription(sid);
            model.Invoices = DataAccess.GetInvoices(model.Subscription.ConsumerBarCode);
            return View(model);
        }

        public IActionResult TransferOwnership()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TransferOwnership(TransferRequestVM vm)
        {
            Subscription sub = DataAccess.GetSubscriptionByBarcode(vm.Subscription.ConsumerBarCode);
            if (sub == null)
            {
                ViewData["message"] = "No subscription with this barcode";
                vm = null;
            }
            else
            {
                vm.Subscription = sub;
                if(vm.Subscription.Consumer == null)
                {
                    ViewData["message"] = "This subscription is not attached .. add attach request";
                }
            }
            return View("TransferOwnership",vm);
        }

        [HttpPost]
        public IActionResult SubmitTransferRequest(TransferRequestVM obj)
        {
            byte[] DocumentBA = null;
            if (obj != null && obj.Document!= null && obj.Subscription != null)
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
                            req.RequestType = "transfer";
                            req.CurrentDepartment = DataAccess.GetDepartments().Where(d => d.Id == 1).FirstOrDefault();
                            req.RequestDate = DateTime.Now;
                            req.Consumer = DataAccess.GetCurrentConsumer();
                            req.RequestStatus = "onprogress";
                            req.Details = new RequestDetails();
                            req.Details.Document = DocumentBA;
                            req.Subscription = DataAccess.GetSubscriptionByBarcode(obj.Subscription.ConsumerBarCode);
                            req = DataAccess.AddRequest(req);
                            if (req != null)
                            {
                                ViewData["message"] = "Request added successfully";
                                return View("Index");
                            }
                            else
                            {
                                ViewData["message"] = "Error submiting the request";
                                return View("TransferOwnership", obj);
                            }
                            
                        }
                        catch (Exception e)
                        {
                            ViewData["message"] = $"Error: {e.Message}";
                            return View("TransferOwnership", obj);
                        }
                    }
                }
                ViewData["message"] = "Document not valid";
                return View("TransferOwnership", obj);
            }
            else
            {
                ViewData["message"] = "Request form not valid";
                return View("TransferOwnership", obj);
            }
        }
    }
}
