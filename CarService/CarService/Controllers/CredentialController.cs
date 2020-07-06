using BusinessLogic;
using BusinessLogic.BLs.Interfaces;
using BusinessLogic.DTOs;
using CarService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CarService.Controllers
{
    public class CredentialController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly ICredentialBL<ClientDTO> _clientBl;
        private readonly ICredentialBL<EmployeeDTO> _employeeBl;

        public CredentialController(IHttpContextAccessor httpContextAccessor, ILogger<EmployeeController> logger, ICredentialBL<ClientDTO> clientBl, ICredentialBL<EmployeeDTO> employeeBl)
        {
            _logger = logger;
            _clientBl = clientBl;
            _employeeBl = employeeBl;
        }

        #region Client

        [Route("SignUp")]
        public ActionResult SignUp()
        {
            var userName = HttpContext.Session.GetString(Constants.SessionKeyUserName);
            if (string.IsNullOrEmpty(userName))
            {
                return View();
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Route("SignUp")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignUp(IFormCollection collection)
        {
            try
            {
                var loginResult = await _clientBl.RegisterAsync(new CredentialDTO
                {
                    EmailAddress = collection["EmailAddress"],
                    Password = collection["Password"],
                });
                if (loginResult.SuccessfulOperation)
                {
                    HttpContext.Session.SetInt32(Constants.SessionKeyUserId, loginResult.Id);
                    HttpContext.Session.SetString(Constants.SessionKeyUserName, loginResult.EmailAddress);
                    HttpContext.Session.SetInt32(Constants.SessionKeyUserRole, (int)loginResult.UserRole);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                return View(CredentialModel.FromDto(loginResult));
            }
            catch
            {
                return View();
            }
        }

        [Route("SignIn")]
        public ActionResult SignIn()
        {
            var userName = HttpContext.Session.GetString(Constants.SessionKeyUserName);
            if (string.IsNullOrEmpty(userName))
            {
                return View();
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Route("SignIn")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(IFormCollection collection)
        {
            try
            {
                var loginResult = _clientBl.LogIn(new CredentialDTO
                {
                    EmailAddress = collection["EmailAddress"],
                    Password = collection["Password"],
                });
                if (loginResult.SuccessfulOperation)
                {
                    if (loginResult.RequirePasswordChange)
                    {
                        return ChangePassword(loginResult.Id);
                    }
                    else
                    {
                        HttpContext.Session.SetInt32(Constants.SessionKeyUserId, loginResult.Id);
                        HttpContext.Session.SetString(Constants.SessionKeyUserName, loginResult.EmailAddress);
                        HttpContext.Session.SetInt32(Constants.SessionKeyUserRole, (int)loginResult.UserRole);
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                }
                return View(CredentialModel.FromDto(loginResult));
            }
            catch
            {
                return View();
            }
        }

        [Route("ChangePassword")]
        public ActionResult ChangePassword(int id)
        {
            return View("ChangePassword", new CredentialModel
            {
                Id = id,
            });
        }

        [Route("ChangePassword")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(int id, IFormCollection collection)
        {
            try
            {
                var dto = new CredentialDTO
                {
                    Id = id,
                    Password = collection["Password"],
                    RepeatPassword = collection["RepeatPassword"],
                };
                if (dto.Password.Equals(dto.RepeatPassword))
                {
                    var changePasswordResult = await _employeeBl.ChangePasswordAsync(dto);
                    if (changePasswordResult)
                    {
                        return RedirectToAction("SignInEmployee");
                    }
                }
                else
                {
                    return View(new CredentialModel
                    {
                        Id = id,
                        ErrorMessage = Constants.RegistrationPasswordsDoNotMatch,
                    });
                }
                return View(new CredentialModel
                {
                    Id = id,
                });
            }
            catch
            {
                return View(new CredentialModel
                {
                    Id = id,
                });
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
        public async Task<ActionResult> ForgottenPassword(IFormCollection collection)
        {
            try
            {
                var dto = new CredentialDTO
                {
                    EmailAddress = collection["EmailAddress"],
                };
                await _clientBl.ForgottenPasswordAsync(dto);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            catch
            {
                return View();
            }
        }

        [Route("ChangeForgottenPassword")]
        public ActionResult ChangeForgottenPassword(string emailAddress, string confirmationToken)
        {
            var userName = HttpContext.Session.GetString(Constants.SessionKeyUserName);
            if (string.IsNullOrEmpty(userName))
            {
                return View(new CredentialModel
                {
                    EmailAddress = emailAddress,
                    ConfirmationToken = confirmationToken,
                });
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Route("ChangeForgottenPassword")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeForgottenPassword(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            catch
            {
                return View(new CredentialModel
                {
                    Id = id,
                });
            }
        }

        #endregion Client

        #region Employee

        [Route("SignInEmployee")]
        public ActionResult SignInEmployee()
        {
            var userName = HttpContext.Session.GetString(Constants.SessionKeyUserName);
            if (string.IsNullOrEmpty(userName))
            {
                return View();
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Route("SignInEmployee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignInEmployee(IFormCollection collection)
        {
            try
            {
                var loginResult = _employeeBl.LogIn(new CredentialDTO
                {
                    EmailAddress = collection["EmailAddress"],
                    Password = collection["Password"],
                });
                if (loginResult.SuccessfulOperation)
                {
                    if (loginResult.RequirePasswordChange)
                    {
                        return ChangePasswordEmployee(loginResult.Id);
                    }
                    else
                    {
                        HttpContext.Session.SetInt32(Constants.SessionKeyUserId, loginResult.Id);
                        HttpContext.Session.SetString(Constants.SessionKeyUserName, loginResult.EmailAddress);
                        HttpContext.Session.SetInt32(Constants.SessionKeyUserRole, (int)loginResult.UserRole);
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                }
                return View(CredentialModel.FromDto(loginResult));
            }
            catch
            {
                return View();
            }
        }

        [Route("ChangePasswordEmployee")]
        public ActionResult ChangePasswordEmployee(int id)
        {
            return View("ChangePasswordEmployee", new CredentialModel
            {
                Id = id,
            });
        }

        [Route("ChangePasswordEmployee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePasswordEmployee(int id, IFormCollection collection)
        {
            try
            {
                var dto = new CredentialDTO
                {
                    Id = id,
                    Password = collection["Password"],
                    RepeatPassword = collection["RepeatPassword"],
                };
                if (dto.Password.Equals(dto.RepeatPassword))
                {
                    var changePasswordResult = await _employeeBl.ChangePasswordAsync(dto);
                    if (changePasswordResult)
                    {
                        return RedirectToAction("SignInEmployee");
                    }
                }
                else
                {
                    return View(new CredentialModel
                    {
                        Id = id,
                        ErrorMessage = Constants.RegistrationPasswordsDoNotMatch,
                    });
                }
                return View(new CredentialModel
                {
                    Id = id,
                });
            }
            catch
            {
                return View(new CredentialModel
                {
                    Id = id,
                });
            }
        }

        [Route("ForgottenPasswordEmployee")]
        public ActionResult ForgottenPasswordEmployee()
        {
            return View();
        }

        [Route("ForgottenPasswordEmployee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgottenPasswordEmployee(IFormCollection collection)
        {
            try
            {
                var dto = new CredentialDTO
                {
                    EmailAddress = collection["EmailAddress"],
                };
                await _employeeBl.ForgottenPasswordAsync(dto);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            catch
            {
                return View();
            }
        }

        [Route("ChangeForgottenPasswordEmployee")]
        public ActionResult ChangeForgottenPasswordEmployee(string emailAddress, string confirmationToken)
        {
            var userName = HttpContext.Session.GetString(Constants.SessionKeyUserName);
            if (string.IsNullOrEmpty(userName))
            {
                return View(new CredentialModel
                {
                    EmailAddress = emailAddress,
                    ConfirmationToken = confirmationToken,
                });
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Route("ChangeForgottenPasswordEmployee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeForgottenPasswordEmployee(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            catch
            {
                return View(new CredentialModel
                {
                    Id = id,
                });
            }
        }

        #endregion Employee

        [Route("LogOut")]
        public ActionResult LogOut()
        {
            var userName = HttpContext.Session.GetString(Constants.SessionKeyUserName);
            if (!string.IsNullOrEmpty(userName))
            {
                HttpContext.Session.Clear();
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
