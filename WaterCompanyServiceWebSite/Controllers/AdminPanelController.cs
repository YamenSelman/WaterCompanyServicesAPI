using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using ModelLibrary;
using System.Collections.Generic;
using WaterCompanyServiceWebSite.Models;

namespace WaterCompanyServiceWebSite.Controllers
{
    public class AdminPanelController : Controller
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
        public IActionResult UserManagement()
        {
            var data = DataAccess.GetUsers().Where(d=>d.UserType.ToLower().Equals("consumer")).ToList();
            return View(data);
        }     
        public IActionResult EmployeeManagement()
        {
            var data = DataAccess.GetUsers().Where(d=>d.UserType.ToLower().Equals("employee")).ToList();
            return View(data);
        }

        public IActionResult EditUser(int id)
        {
            User user = DataAccess.GetUser(id);
            user.AccountActive = !user.AccountActive;
            DataAccess.UpdateUser(user);
            return RedirectToAction("UserManagement", "AdminPanel");
        }       
        public IActionResult EditEmployee(int id)
        {
            User user = DataAccess.GetUser(id);
            user.AccountActive = !user.AccountActive;
            DataAccess.UpdateUser(user);
            return RedirectToAction("EmployeeManagement", "AdminPanel");
        }        
        
        public IActionResult ResetPassword(int id)
        {
            User user = DataAccess.GetUser(id);
            user.Password = "0000";
            DataAccess.UpdateUser(user);
            return RedirectToAction("EmployeeManagement", "AdminPanel");
        }

        public IActionResult AddEmployee()
        {
            var deps = DataAccess.GetDepartments();
            ViewData["departments"] = deps;
            return View();
        }

        [HttpPost]
        public IActionResult AddEmployee(Employee employee)
        {
            if (DataAccess.UserNameExists(employee.User.UserName))
            {
                ViewData["message"] = "User Name Exists, please use another User Name";
                var deps = DataAccess.GetDepartments();
                ViewBag.data = deps;
                return View(employee);
            }
            
            employee.User.AccountActive = true;
            employee.User.UserType = "employee";
            DataAccess.AddEmployee(employee);
            return RedirectToAction("EmployeeManagement", "AdminPanel");
        }

        public IActionResult DashBoard()
        {
            if(DataAccess.SubscriptionForecast is null)
            {
                DataAccess.DoForecast();
            }

            var requests = DataAccess.GetAllRequests();

            DashboardVM vm = new DashboardVM();

            vm.subscriptionForecast = DataAccess.SubscriptionForecast;
            vm.rejectedPer = (float)requests.Where(r => r.RequestStatus.Equals("rejected")).Count() / (float)requests.Count;
            vm.completedPer = (float)requests.Where(r => r.RequestStatus.Equals("completed")).Count() / (float)requests.Count;
            vm.onprogressPer = (float)requests.Where(r => r.RequestStatus.Equals("onprogress")).Count() / (float)requests.Count;
            vm.rejectedCount = requests.Where(r => r.RequestStatus.Equals("rejected")).Count();
            vm.completedCount = requests.Where(r => r.RequestStatus.Equals("completed")).Count();
            vm.onprogressCount = requests.Where(r => r.RequestStatus.Equals("onprogress")).Count();


            vm.totalRequests = requests.Count;

            return View(vm);
        }

    }
}
