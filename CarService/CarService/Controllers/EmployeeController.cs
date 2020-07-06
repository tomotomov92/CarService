using BusinessLogic;
using BusinessLogic.BLs.Interfaces;
using BusinessLogic.DTOs;
using BusinessLogic.Enums;
using CarService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CarService.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IBaseBL<EmployeeDTO> _bl;
        private readonly IBaseBL<EmployeeRoleDTO> _employeeRoleBl;

        public EmployeeController(IHttpContextAccessor httpContextAccessor, ILogger<EmployeeController> logger, ICredentialBL<EmployeeDTO> bl, IBaseBL<EmployeeRoleDTO> employeeRoleBl)
        {
            _logger = logger;
            _bl = bl;
            _employeeRoleBl = employeeRoleBl;
        }

        public ActionResult Index()
        {
            var userRoleValue = HttpContext.Session.GetInt32(Constants.SessionKeyUserRole);
            if (userRoleValue != null)
            {
                var userRole = (UserRoles)userRoleValue;
                if (userRole == UserRoles.Owner ||
                    userRole == UserRoles.CustomerSupport)
                {
                    var resultsAsDTO = _bl.ReadAll();
                    var resultsAsModel = EmployeeModel.FromDtos(resultsAsDTO);
                    return View(resultsAsModel);
                }
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public ActionResult Details(int id)
        {
            var userRoleValue = HttpContext.Session.GetInt32(Constants.SessionKeyUserRole);
            if (userRoleValue != null)
            {
                var userRole = (UserRoles)userRoleValue;
                if (userRole == UserRoles.Owner ||
                    userRole == UserRoles.CustomerSupport)
                {
                    return GetRecordById(id);
                }
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public ActionResult Create()
        {
            var userRoleValue = HttpContext.Session.GetInt32(Constants.SessionKeyUserRole);
            if (userRoleValue != null)
            {
                var userRole = (UserRoles)userRoleValue;
                if (userRole == UserRoles.Owner ||
                    userRole == UserRoles.CustomerSupport)
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
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                await _bl.CreateAsync(new EmployeeDTO
                {
                    FirstName = collection["FirstName"],
                    LastName = collection["LastName"],
                    EmailAddress = collection["EmailAddress"],
                    Password = Constants.DefaultPassword,
                    DateOfStart = DateTime.Parse(collection["DateOfStart"]),
                    EmployeeRoleId = int.Parse(collection["EmployeeRoleId"]),
                });
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Create();
            }
        }

        public ActionResult Edit(int id)
        {
            var userRoleValue = HttpContext.Session.GetInt32(Constants.SessionKeyUserRole);
            if (userRoleValue != null)
            {
                var userRole = (UserRoles)userRoleValue;
                if (userRole == UserRoles.Owner ||
                    userRole == UserRoles.CustomerSupport)
                {
                    return GetRecordById(id);
                }
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
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
                    Archived = bool.Parse(collection["Archived"][0]),
                });
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return GetRecordById(id);
            }
        }

        public ActionResult Delete(int id)
        {
            var userRoleValue = HttpContext.Session.GetInt32(Constants.SessionKeyUserRole);
            if (userRoleValue != null)
            {
                var userRole = (UserRoles)userRoleValue;
                if (userRole == UserRoles.Owner ||
                    userRole == UserRoles.CustomerSupport)
                {
                    return GetRecordById(id);
                }
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
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
                return GetRecordById(id);
            }
        }

        private ActionResult GetRecordById(int id)
        {
            var activeEmployeeRoles = _employeeRoleBl.ReadActive();
            var activeEmployeeRolesAsModel = EmployeeRoleModel.FromDtos(activeEmployeeRoles);
            var employeeRolesOptions = new SelectList(activeEmployeeRolesAsModel, nameof(EmployeeRoleModel.Id), nameof(EmployeeRoleModel.EmployeeRoleName));

            var resultAsDTO = _bl.ReadById(id);
            var resultAsModel = EmployeeModel.FromDto(resultAsDTO);
            resultAsModel.EmployeeRoleOptions = employeeRolesOptions;
            return View(resultAsModel);
        }
    }
}
