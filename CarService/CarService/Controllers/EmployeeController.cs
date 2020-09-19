using BusinessLogic;
using BusinessLogic.BLs.Interfaces;
using BusinessLogic.DTOs;
using BusinessLogic.Enums;
using CarService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CarService.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmployeeController> _logger;
        private readonly ICredentialBL<EmployeeDTO> _bl;
        private readonly IBaseBL<EmployeeRoleDTO> _employeeRoleBl;
        private readonly UserRoles _userRole = UserRoles.NA;

        public EmployeeController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment, IConfiguration configuration, ILogger<EmployeeController> logger, ICredentialBL<EmployeeDTO> bl, IBaseBL<EmployeeRoleDTO> employeeRoleBl)
        {
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _logger = logger;
            _bl = bl;
            _employeeRoleBl = employeeRoleBl;

            var userRoleInt = httpContextAccessor.HttpContext.Session.GetInt32(Constants.SessionKeyUserRole);
            if (userRoleInt != null)
            {
                _userRole = (UserRoles)userRoleInt;
            }
        }

        public ActionResult Index()
        {
            switch (_userRole)
            {
                case UserRoles.Owner:
                case UserRoles.CustomerSupport:
                    {
                        var resultsAsDTO = _bl.ReadAll();
                        var resultsAsModel = EmployeeModel.FromDtos(resultsAsDTO);
                        return View(resultsAsModel);
                    }
                default:
                    return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        public ActionResult Details(int id)
        {
            switch (_userRole)
            {
                case UserRoles.Owner:
                case UserRoles.CustomerSupport:
                    return GetActionForRecordById(id);
                default:
                    return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        public ActionResult Create()
        {
            switch (_userRole)
            {
                case UserRoles.Owner:
                    {
                        var activeEmployeeRoles = _employeeRoleBl.ReadActive();
                        var activeEmployeeRolesAsModel = EmployeeRoleModel.FromDtos(activeEmployeeRoles);
                        var employeeRolesOptions = new SelectList(activeEmployeeRolesAsModel, nameof(EmployeeRoleModel.Id), nameof(EmployeeRoleModel.EmployeeRoleName));

                        var model = new EmployeeModel
                        {
                            EmployeeRoleId = activeEmployeeRoles.First().Id,
                            EmployeeRoleOptions = employeeRolesOptions,
                        };
                        return View(model);
                    }
                default:
                    return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                var dto = new EmployeeDTO
                {
                    FirstName = collection["FirstName"],
                    LastName = collection["LastName"],
                    EmailAddress = collection["EmailAddress"],
                    Password = collection["Password"],
                    RepeatPassword = collection["RepeatPassword"],
                    DateOfStart = DateTime.Parse(collection["DateOfStart"]),
                    EmployeeRoleId = int.Parse(collection["EmployeeRoleId"]),
                    EmployeeRole = _employeeRoleBl.ReadById(int.Parse(collection["EmployeeRoleId"])),
                };

                var result = await _bl.RegisterAsync(dto, _webHostEnvironment.ContentRootPath, _configuration.GetValue<string>("BASE_URL"), false);
                if (result.SuccessfulOperation)
                {
                    return RedirectToAction(nameof(Index));
                }
                var employeeDTO = EmployeeDTO.FromCredentialDTO(result);
                var employeeModel = EmployeeModel.FromDto(employeeDTO);
                employeeModel.EmployeeRoleOptions = GetEmployeeRoleOptions();
                return View(employeeModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(EmployeeController.Create));
                return Create();
            }
        }

        public ActionResult Edit(int id)
        {
            switch (_userRole)
            {
                case UserRoles.Owner:
                    return GetActionForRecordById(id);
                default:
                    return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, IFormCollection collection)
        {
            try
            {
                await _bl.UpdateAsync(new EmployeeDTO
                {
                    Id = id,
                    FirstName = collection["FirstName"],
                    LastName = collection["LastName"],
                    EmailAddress = collection["EmailAddress"],
                    Password = collection["Password"],
                    EmployeeRoleId = int.Parse(collection["EmployeeRoleId"]),
                });
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(EmployeeController.Edit));
                return GetActionForRecordById(id);
            }
        }

        public async Task<ActionResult> Archive(int id)
        {
            try
            {
                switch (_userRole)
                {
                    case UserRoles.Owner:
                        {
                            await _bl.ArchiveAsync(new EmployeeDTO
                            {
                                Id = id,
                                Archived = true,
                            });
                            return RedirectToAction(nameof(Index));
                        }
                    default:
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(EmployeeController.Archive));
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<ActionResult> Unarchive(int id)
        {
            try
            {
                switch (_userRole)
                {
                    case UserRoles.Owner:
                        {
                            await _bl.ArchiveAsync(new EmployeeDTO
                            {
                                Id = id,
                                Archived = false,
                            });
                            return RedirectToAction(nameof(Index));
                        }
                    default:
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(EmployeeController.Unarchive));
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Delete(int id)
        {
            switch (_userRole)
            {
                case UserRoles.Owner:
                    return GetActionForRecordById(id);
                default:
                    return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                await _bl.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(EmployeeController.Delete));
                return GetActionForRecordById(id);
            }
        }

        private ActionResult GetActionForRecordById(int id)
        {
            var resultAsDTO = _bl.ReadById(id);
            var resultAsModel = EmployeeModel.FromDto(resultAsDTO);
            resultAsModel.EmployeeRoleOptions = GetEmployeeRoleOptions();
            return View(resultAsModel);
        }

        private SelectList GetEmployeeRoleOptions()
        {
            var activeEmployeeRoles = _employeeRoleBl.ReadActive();
            var activeEmployeeRolesAsModel = EmployeeRoleModel.FromDtos(activeEmployeeRoles);
            var employeeRolesOptions = new SelectList(activeEmployeeRolesAsModel, nameof(EmployeeRoleModel.Id), nameof(EmployeeRoleModel.EmployeeRoleName));
            return employeeRolesOptions;
        }
    }
}
