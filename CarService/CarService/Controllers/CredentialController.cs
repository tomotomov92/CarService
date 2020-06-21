using BusinessLogic;
using BusinessLogic.BLs.Interfaces;
using BusinessLogic.DTOs;
using BusinessLogic.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CarService.Controllers
{
    public class CredentialController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IBaseBL<EmployeeDTO> _employeeBl;

        public CredentialController(IHttpContextAccessor httpContextAccessor, ILogger<EmployeeController> logger, IBaseBL<EmployeeDTO> employeeBl)
        {
            _logger = logger;
            _employeeBl = employeeBl;
        }

        [Route("LogIn")]
        public ActionResult LogIn()
        {
            var userName = HttpContext.Session.GetString(Constants.SessionKeyUserName);
            if (string.IsNullOrEmpty(userName))
            {
                return View();
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Route("LogIn")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(IFormCollection collection)
        {
            try
            {
                if (true)
                {
                    HttpContext.Session.SetString(Constants.SessionKeyUserName, collection["EmailAddress"]);
                    HttpContext.Session.SetInt32(Constants.SessionKeyUserRole, (int)UserRoles.Customer);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        [Route("LogInEmployee")]
        public ActionResult LogInEmployee()
        {
            return View();
        }

        [Route("LogInEmployee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogInEmployee(IFormCollection collection)
        {
            try
            {
                if (true)
                {
                    HttpContext.Session.SetString(Constants.SessionKeyUserName, collection["EmailAddress"]);
                    HttpContext.Session.SetInt32(Constants.SessionKeyUserRole, (int)UserRoles.Owner);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        [Route("LogOut")]
        public ActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Route("ChangePassword")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Route("ChangePassword")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(IFormCollection collection)
        {
            try
            {
                return View();
            }
            catch
            {
                return View();
            }
        }

        [Route("ForgottenPassword")]
        public ActionResult ForgottenPassword()
        {
            return View();
        }

        [Route("ForgottenPassword")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgottenPassword(IFormCollection collection)
        {
            try
            {
                return View();
            }
            catch
            {
                return View();
            }
        }

        [Route("ChangeForgottenPassword")]
        public ActionResult ChangeForgottenPassword()
        {
            return View();
        }

        [Route("ChangeForgottenPassword")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeForgottenPassword(IFormCollection collection)
        {
            try
            {
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
