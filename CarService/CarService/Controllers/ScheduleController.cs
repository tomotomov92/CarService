using BusinessLogic.BLs;
using BusinessLogic.DTOs;
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

        public ScheduleController(ILogger<ScheduleController> logger, IBaseBL<ScheduleDTO> bl)
        {
            _logger = logger;
            _bl = bl;
        }

        public async Task<ActionResult> Index()
        {
            var resultsAsDTO = await _bl.GetAllAsync();
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
                await _bl.AddAsync(new ScheduleDTO
                {
                    DateBegin = DateTime.Parse(collection["DateBegin"]),
                    DateEnd = DateTime.Parse(collection["DateEnd"]),
                    EmployeeId = int.Parse(collection["EmployeeId"]),
                });
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
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
                await _bl.UpdateAsync(new ScheduleDTO
                {
                    Id = id,
                    DateBegin = DateTime.Parse(collection["DateBegin"]),
                    DateEnd = DateTime.Parse(collection["DateEnd"]),
                    EmployeeId = int.Parse(collection["EmployeeId"]),
                });
                return RedirectToAction(nameof(Index));
            }
            catch
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
            catch
            {
                return GetRecordById(id);
            }
        }

        private ActionResult GetRecordById(int id)
        {
            var resultAsDTO = _bl.Get(id);
            var resultAsModel = ScheduleModel.FromDto(resultAsDTO);
            return View(resultAsModel);
        }
    }
}
