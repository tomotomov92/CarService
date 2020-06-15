﻿using BusinessLogic.BLs;
using BusinessLogic.DTOs;
using CarService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CarService.Controllers
{
    public class ClientCarController : Controller
    {
        private readonly ILogger<ClientCarController> _logger;
        private readonly IBaseBL<ClientCarDTO> _bl;
        private readonly IBaseBL<CarBrandDTO> _carBrandBl;

        public ClientCarController(ILogger<ClientCarController> logger, IBaseBL<ClientCarDTO> bl, IBaseBL<CarBrandDTO> carBrandBl)
        {
            _logger = logger;
            _bl = bl;
            _carBrandBl = carBrandBl;
        }

        public async Task<ActionResult> Index()
        {
            var resultsAsDTO = await _bl.GetAllAsync();
            var resultsAsModel = ClientCarModel.FromDtos(resultsAsDTO);
            return View(resultsAsModel);
        }

        public ActionResult Details(int id)
        {
            return GetRecordById(id);
        }

        public async Task<ActionResult> Create(int clientId)
        {
            var activeEmployeeRoles = await _carBrandBl.GetAllActiveAsync();
            var employeeRolesOptions = new SelectList(activeEmployeeRoles, nameof(CarBrandModel.Id), nameof(CarBrandModel.BrandName));
            var model = new ClientCarModel
            {
                ClientId = clientId,
                CarBrandOptions = employeeRolesOptions,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                await _bl.AddAsync(new ClientCarDTO
                {
                    ClientId = int.Parse(collection["ClientId"]),
                    CarBrandId = int.Parse(collection["CarBrandId"]),
                    LicensePlate = collection["LicensePlate"],
                    Mileage = int.Parse(collection["Mileage"]),
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
                await _bl.UpdateAsync(new ClientCarDTO
                {
                    Id = id,
                    ClientId = int.Parse(collection["ClientId"]),
                    CarBrandId = int.Parse(collection["CarBrandId"]),
                    LicensePlate = collection["LicensePlate"],
                    Mileage = int.Parse(collection["Mileage"]),
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
            var resultAsModel = ClientCarModel.FromDto(resultAsDTO);
            return View(resultAsModel);
        }
    }
}
