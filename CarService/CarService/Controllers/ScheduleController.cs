using BusinessLogic;
using BusinessLogic.BLs.Interfaces;
using BusinessLogic.DTOs;
using BusinessLogic.Enums;
using CarService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CarService.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly IBaseBL<ScheduleDTO> _bl;
        private readonly UserRoles _userRole = UserRoles.NA;

        public ScheduleController(IHttpContextAccessor httpContextAccessor, ILogger<ScheduleController> logger, IBaseBL<ScheduleDTO> bl)
        {
            _logger = logger;
            _bl = bl;

            var userRoleInt = httpContextAccessor.HttpContext.Session.GetInt32(Constants.SessionKeyUserRole);
            if (userRoleInt != null)
            {
                _userRole = (UserRoles)userRoleInt;
            }
        }

        public ActionResult Index()
        {
            var resultsAsDTO = _bl.ReadAll();
            var resultsAsModel = ScheduleModel.FromDtos(resultsAsDTO);
            return View(resultsAsModel);
        }

        public ActionResult Details(int id)
        {
            return GetRecordById(id);
        }

        public ActionResult Create(int employeeId)
        {
            var model = new ScheduleModel
            {
                EmployeeId = employeeId,
            };
            return View(model);
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
                return Create(employeeId: int.Parse(collection["EmployeeId"]));
            }
        }

        public ActionResult Edit(int id)
        {
            return GetRecordById(id);
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
                return GetRecordById(id);
            }
        }

        public ActionResult Delete(int id)
        {
            return GetRecordById(id);
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
            var resultAsDTO = _bl.ReadById(id);
            var resultAsModel = ScheduleModel.FromDto(resultAsDTO);
            return View(resultAsModel);
        }
    }
}
