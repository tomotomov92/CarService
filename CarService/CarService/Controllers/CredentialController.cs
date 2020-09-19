using BusinessLogic;
using BusinessLogic.BLs.Interfaces;
using BusinessLogic.DTOs;
using CarService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CarService.Controllers
{
    public class CredentialController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmployeeController> _logger;
        private readonly ICredentialBL<ClientDTO> _clientBl;
        private readonly ICredentialBL<EmployeeDTO> _employeeBl;
        private readonly string _userName;

        public CredentialController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment, IConfiguration configuration, ILogger<EmployeeController> logger, ICredentialBL<ClientDTO> clientBl, ICredentialBL<EmployeeDTO> employeeBl)
        {
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _logger = logger;
            _clientBl = clientBl;
            _employeeBl = employeeBl;

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
                var dto = new CredentialDTO
                {
                    FirstName = collection["FirstName"],
                    LastName = collection["LastName"],
                    EmailAddress = collection["EmailAddress"],
                    Password = collection["Password"],
                    RepeatPassword = collection["RepeatPassword"],
                };

                var result = await _clientBl.RegisterAsync(dto, _webHostEnvironment.ContentRootPath, _configuration.GetValue<string>("BASE_URL"), true);
                if (result.SuccessfulOperation)
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                return View(CredentialModel.FromDto(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CredentialController.SignUp));
                return View();
            }
        }

        [Route("ConfirmAccount")]
        public async Task<ActionResult> ConfirmAccount(string emailAddress, string confirmationToken)
        {
            if (string.IsNullOrEmpty(_userName))
            {
                var clientResult = _clientBl.ReadByEmailAddress(emailAddress);
                var activeTokens = _clientBl.ReadActiveTokenByUserId(new TokenDTO
                {
                    ClientId = clientResult.Id
                });
                if (activeTokens.Any(x => x.Token.Equals(confirmationToken)))
                {
                    clientResult.Activated = true;
                    var tokenDto = activeTokens.First(x => x.Token.Equals(confirmationToken));
                    tokenDto.IsValid = false;
                    await _clientBl.ConfirmAccountAsync(clientResult, tokenDto);
                    return RedirectToAction(nameof(CredentialController.SignIn), "Credential");
                }
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
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
                
                var result = await _clientBl.ForgottenPasswordAsync(dto, _webHostEnvironment.ContentRootPath, _configuration.GetValue<string>("BASE_URL"));
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CredentialController.ForgottenPassword));
                return View();
            }
        }

        [Route("ChangeForgottenPassword")]
        public async Task<ActionResult> ChangeForgottenPassword(string emailAddress, string confirmationToken)
        {
            if (string.IsNullOrEmpty(_userName))
            {
                var clientResult = _clientBl.ReadByEmailAddress(emailAddress);
                var activeTokens = _clientBl.ReadActiveTokenByUserId(new TokenDTO
                {
                    ClientId = clientResult.Id
                });
                if (activeTokens.Any(x => x.Token.Equals(confirmationToken)))
                {
                    var tokenDto = activeTokens.First(x => x.Token.Equals(confirmationToken));
                    tokenDto.IsValid = false;
                    await _clientBl.UpdateTokenAsync(tokenDto);

                    return View("ChangePassword", new CredentialModel
                    {
                        Id = clientResult.Id
                    }) ;
                }
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
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

                var clientToken = await _employeeBl.ForgottenPasswordAsync(dto, _webHostEnvironment.ContentRootPath, _configuration.GetValue<string>("BASE_URL"));
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CredentialController.ForgottenPassword));
                return View();
            }
        }

        [Route("ChangeForgottenPasswordEmployee")]
        public async Task<ActionResult> ChangeForgottenPasswordEmployee(string emailAddress, string confirmationToken)
        {
            if (string.IsNullOrEmpty(_userName))
            {
                var employeeResult = _employeeBl.ReadByEmailAddress(emailAddress);
                var activeTokens = _employeeBl.ReadActiveTokenByUserId(new TokenDTO
                {
                    ClientId = employeeResult.Id
                });
                if (activeTokens.Any(x => x.Token.Equals(confirmationToken)))
                {
                    var tokenDto = activeTokens.First(x => x.Token.Equals(confirmationToken));
                    tokenDto.IsValid = false;
                    await _employeeBl.UpdateTokenAsync(tokenDto);

                    return View("ChangePasswordEmployee", new CredentialModel
                    {
                        Id = employeeResult.Id
                    });
                }
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
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
