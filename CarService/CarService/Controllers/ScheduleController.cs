﻿using BusinessLogic;
using BusinessLogic.BLs;
using BusinessLogic.BLs.Interfaces;
using BusinessLogic.DTOs;
using BusinessLogic.Enums;
using CarService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CarService.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly IBaseBL<ScheduleDTO> _bl;
        private readonly ICredentialBL<EmployeeDTO> _employeeBl;
        private readonly UserRoles _userRole = UserRoles.NA;

        public ScheduleController(IHttpContextAccessor httpContextAccessor, ILogger<ScheduleController> logger, IBaseBL<ScheduleDTO> bl, ICredentialBL<EmployeeDTO> employeeBL)
        {
            _logger = logger;
            _bl = bl;
            _employeeBl = employeeBL;

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
                case UserRoles.Mechanic:
                case UserRoles.CustomerSupport:
                    {
                        var resultsAsDTO = _bl.ReadAll();
                        var resultsAsModel = ScheduleModel.FromDtos(resultsAsDTO);
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
                case UserRoles.Mechanic:
                case UserRoles.CustomerSupport:
                    return GetActionForRecordById(id);
                default:
                    return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        public ActionResult Create(int employeeId)
        {
            switch (_userRole)
            {
                case UserRoles.Owner:
                case UserRoles.CustomerSupport:
                    {
                        var model = new ScheduleModel
                        {
                            EmployeeId = employeeId,
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
                var scheduleDate = DateTime.Parse(collection["ScheduleDate"]);
                var scheduleStartTime = TimeSpan.Parse(collection["ScheduleStartTime"]);
                var scheduleEndTime = TimeSpan.Parse(collection["ScheduleEndTime"]);

                var dateBegin = scheduleDate + scheduleStartTime;
                var dateEnd = scheduleDate + scheduleEndTime;

                await _bl.CreateAsync(new ScheduleDTO
                {
                    DateBegin = dateBegin,
                    DateEnd = dateEnd,
                    EmployeeId = int.Parse(collection["EmployeeId"]),
                });
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(EmployeeController.Create));
                return Create(employeeId: int.Parse(collection["EmployeeId"]));
            }
        }

        public ActionResult Edit(int id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, IFormCollection collection)
        {
            try
            {
                var scheduleDate = DateTime.Parse(collection["ScheduleDate"]);
                var scheduleStartTime = TimeSpan.Parse(collection["ScheduleStartTime"]);
                var scheduleEndTime = TimeSpan.Parse(collection["ScheduleEndTime"]);

                var dateBegin = scheduleDate + scheduleStartTime;
                var dateEnd = scheduleDate + scheduleEndTime;

                await _bl.UpdateAsync(new ScheduleDTO
                {
                    Id = id,
                    DateBegin = dateBegin,
                    DateEnd = dateEnd,
                    EmployeeId = int.Parse(collection["EmployeeId"]),
                });
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(EmployeeController.Edit));
                return GetActionForRecordById(id);
            }
        }

        public ActionResult Delete(int id)
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
            var activeEmployees = _employeeBl.ReadActive();
            var activeEmployeesAsModel = EmployeeModel.FromDtos(activeEmployees);
            var employeeOptions = new SelectList(activeEmployeesAsModel, nameof(EmployeeModel.Id), nameof(EmployeeModel.FullName));

            var resultAsDTO = _bl.ReadById(id);
            var resultAsModel = ScheduleModel.FromDto(resultAsDTO);
            resultAsModel.EmployeeOptions = employeeOptions;
            return View(resultAsModel);
        }
    }
}
