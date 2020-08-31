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
    public class InspectionController : Controller
    {
        private readonly ILogger<InspectionController> _logger;
        private readonly IInspectionBL<InspectionDTO> _bl;
        private readonly UserRoles _userRole = UserRoles.NA;
        private readonly int _userId = 0;

        public InspectionController(IHttpContextAccessor httpContextAccessor, ILogger<InspectionController> logger, IInspectionBL<InspectionDTO> bl)
        {
            _logger = logger;
            _bl = bl;

            var userRoleInt = httpContextAccessor.HttpContext.Session.GetInt32(Constants.SessionKeyUserRole);
            if (userRoleInt != null)
            {
                _userRole = (UserRoles)userRoleInt;
            }
            _userId = httpContextAccessor.HttpContext.Session.GetInt32(Constants.SessionKeyUserId).Value;
        }

        public ActionResult Index()
        {
            var resultsAsDTO = _bl.ReadAll();
            var resultsAsModel = InspectionModel.FromDtos(resultsAsDTO);
            return View(resultsAsModel);
        }

        public ActionResult Details(int id)
        {
            return GetRecordById(id);
        }

        public ActionResult Create(int clientId, int carId)
        {
            var model = new InspectionModel
            {
                ClientId = clientId,
                CarId = carId,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                var dateForInspection = DateTime.Parse(collection["DateForInspection"]);
                var timeForInspection = TimeSpan.Parse(collection["TimeForInspection"]);

                var dateTimeOfInspection = dateForInspection + timeForInspection;

                await _bl.CreateAsync(new InspectionDTO
                {
                    ClientId = int.Parse(collection["ClientId"]),
                    CarId = int.Parse(collection["CarId"]),
                    Mileage = int.Parse(collection["Mileage"]),
                    DateTimeOfInspection = dateTimeOfInspection,
                    Description = collection["Description"],
                });

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Create(clientId: int.Parse(collection["ClientId"]), carId: int.Parse(collection["CarId"]));
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
                var dateForInspection = DateTime.Parse(collection["DateForInspection"]);
                var timeForInspection = TimeSpan.Parse(collection["TimeForInspection"]);

                var dateTimeOfInspection = dateForInspection + timeForInspection;

                await _bl.UpdateAsync(new InspectionDTO
                {
                    Id = id,
                    ClientId = int.Parse(collection["ClientId"]),
                    CarId = int.Parse(collection["CarId"]),
                    Mileage = int.Parse(collection["Mileage"]),
                    DateTimeOfInspection = dateTimeOfInspection,
                    Description = collection["Description"],
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

        public ActionResult EmployeeInspections()
        {
            if (_userRole == UserRoles.Mechanic)
            {
                return EmployeeInspections(_userId);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Route("EmployeeInspections")]
        public ActionResult EmployeeInspections(int employeeId)
        {
            var resultsAsDTO = _bl.ReadForEmployeeId(employeeId);
            var resultsAsModel = InspectionModel.FromDtos(resultsAsDTO);
            return View("Index", resultsAsModel);
        }

        [Route("ClientInspections")]
        public ActionResult ClientInspections()
        {
            if (_userRole == UserRoles.Customer)
            {
                return ClientInspections(_userId);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public ActionResult ClientInspections(int clientId)
        {
            var resultsAsDTO = _bl.ReadForClientId(clientId);
            var resultsAsModel = InspectionModel.FromDtos(resultsAsDTO);
            return View("Index", resultsAsModel);
        }


        public ActionResult CarInspections(int carId)
        {
            var resultsAsDTO = _bl.ReadForCarId(carId);
            var resultsAsModel = InspectionModel.FromDtos(resultsAsDTO);
            return View("Index", resultsAsModel);
        }

        private ActionResult GetRecordById(int id)
        {
            var resultAsDTO = _bl.ReadById(id);
            var resultAsModel = InspectionModel.FromDto(resultAsDTO);
            return View(resultAsModel);
        }
    }
}
