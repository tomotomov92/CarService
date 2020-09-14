using BusinessLogic;
using BusinessLogic.BLs.Interfaces;
using BusinessLogic.DTOs;
using BusinessLogic.EmailSender;
using CarService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CarService.Controllers
{
    public class CredentialController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<EmployeeController> _logger;
        private readonly ICredentialBL<ClientDTO> _clientBl;
        private readonly ICredentialBL<EmployeeDTO> _employeeBl;
        private readonly EmailSender _emailSender;
        private readonly string _userName;

        public CredentialController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment, ILogger<EmployeeController> logger, ICredentialBL<ClientDTO> clientBl, ICredentialBL<EmployeeDTO> employeeBl, EmailSender emailSender)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _clientBl = clientBl;
            _employeeBl = employeeBl;
            _emailSender = emailSender;

            _userName = httpContextAccessor.HttpContext.Session.GetString(Constants.SessionKeyUserName);
        }

        #region Client

        [Route("SignUp")]
        public ActionResult SignUp()
        {
            if (string.IsNullOrEmpty(_userName))
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
                    FirstName = collection["FirstName"],
                    LastName = collection["LastName"],
                    EmailAddress = collection["EmailAddress"],
                    Password = collection["Password"],
                    RepeatPassword = collection["RepeatPassword"],
                }, _webHostEnvironment.WebRootPath);
                if (loginResult.SuccessfulOperation)
                {
                    HttpContext.Session.SetInt32(Constants.SessionKeyUserId, loginResult.Id);
                    HttpContext.Session.SetString(Constants.SessionKeyUserName, loginResult.EmailAddress);
                    HttpContext.Session.SetInt32(Constants.SessionKeyUserRole, (int)loginResult.UserRole);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                return View(CredentialModel.FromDto(loginResult));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CredentialController.SignUp));
                return View();
            }
        }

        [Route("SignIn")]
        public ActionResult SignIn()
        {
            if (string.IsNullOrEmpty(_userName))
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
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CredentialController.SignIn));
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
                    var changePasswordResult = await _clientBl.ChangePasswordAsync(dto);
                    if (changePasswordResult)
                    {
                        return RedirectToAction("SignIn");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CredentialController.ChangePassword));
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
                
                var clientToken = await _clientBl.ForgottenPasswordAsync(dto, _webHostEnvironment.WebRootPath);
                if (clientToken != null)
                {
                    _emailSender.SendEmail(clientToken.EmailAddress, clientToken.EmailSubject, clientToken.EmailBody);
                }
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CredentialController.ForgottenPassword));
                return View();
            }
        }

        [Route("ChangeForgottenPassword")]
        public ActionResult ChangeForgottenPassword(string emailAddress, string confirmationToken)
        {
            if (string.IsNullOrEmpty(_userName))
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
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CredentialController.ChangeForgottenPassword));
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
            if (string.IsNullOrEmpty(_userName))
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
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CredentialController.SignInEmployee));
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
                    RequirePasswordChange = false,
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
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CredentialController.ChangePasswordEmployee));
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

                var clientToken = await _employeeBl.ForgottenPasswordAsync(dto, _webHostEnvironment.WebRootPath);
                if (clientToken != null)
                {
                    _emailSender.SendEmail(clientToken.EmailAddress, clientToken.EmailSubject, clientToken.EmailBody);
                }
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CredentialController.ForgottenPassword));
                return View();
            }
        }

        [Route("ChangeForgottenPasswordEmployee")]
        public ActionResult ChangeForgottenPasswordEmployee(string emailAddress, string confirmationToken)
        {
            if (string.IsNullOrEmpty(_userName))
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
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CredentialController.ChangeForgottenPassword));
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
            if (!string.IsNullOrEmpty(_userName))
            {
                HttpContext.Session.Clear();
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
