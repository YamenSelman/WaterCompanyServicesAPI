using Microsoft.AspNetCore.Mvc;
using ModelLibrary;

namespace WaterCompanyServiceWebSite.Controllers
{
    public class AdminPanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logout()
        {
            DataAccess.CurrentUser = null;
            return RedirectToAction("Index","Home");
        }

        public IActionResult UserManagement()
        {
            var data = DataAccess.GetUsers();
            return View(data);
        }

        public IActionResult EditUser(int id)
        {
            User user = DataAccess.GetUser(id);
            user.AccountActive = !user.AccountActive;
            DataAccess.UpdateUser(user);
            return RedirectToAction("UserManagement", "AdminPanel");
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
            return RedirectToAction("UserManagement", "AdminPanel");
        }

    }
}
