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
    public class InspectionController : Controller
    {
        private readonly ILogger<InspectionController> _logger;
        private readonly IBaseBL<InspectionDTO> _bl;

        public InspectionController(ILogger<InspectionController> logger, IBaseBL<InspectionDTO> bl)
        {
            _logger = logger;
            _bl = bl;
        }

        public async Task<ActionResult> Index()
        {
            var resultsAsDTO = await _bl.GetAllAsync();
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
                await _bl.AddAsync(new InspectionDTO
                {
                    ClientId = int.Parse(collection["ClientId"]),
                    CarId = int.Parse(collection["CarId"]),
                    Mileage = int.Parse(collection["Mileage"]),
                    DateTimeOfInspection = DateTime.Parse(collection["DateTimeOfInspection"]),
                    Description = collection["Description"],
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
                await _bl.UpdateAsync(new InspectionDTO
                {
                    Id = id,
                    ClientId = int.Parse(collection["ClientId"]),
                    CarId = int.Parse(collection["CarId"]),
                    Mileage = int.Parse(collection["Mileage"]),
                    DateTimeOfInspection = DateTime.Parse(collection["DateTimeOfInspection"]),
                    Description = collection["Description"],
                    Archived = bool.Parse(collection["Archived"][0]),
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
            var resultAsModel = InspectionModel.FromDto(resultAsDTO);
            return View(resultAsModel);
        }
    }
}
