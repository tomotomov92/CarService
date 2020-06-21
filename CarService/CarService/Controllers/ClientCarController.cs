using BusinessLogic.BLs.Interfaces;
using BusinessLogic.DTOs;
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
    public class ClientCarController : Controller
    {
        private readonly ILogger<ClientCarController> _logger;
        private readonly IClientCarBL<ClientCarDTO> _bl;
        private readonly IBaseBL<CarBrandDTO> _carBrandBl;
        private readonly IBaseBL<ClientDTO> _clientBl;

        public ClientCarController(IHttpContextAccessor httpContextAccessor, ILogger<ClientCarController> logger, IClientCarBL<ClientCarDTO> bl, IBaseBL<CarBrandDTO> carBrandBl, IBaseBL<ClientDTO> clientBl)
        {
            _logger = logger;
            _bl = bl;
            _carBrandBl = carBrandBl;
            _clientBl = clientBl;
        }

        public ActionResult Index()
        {
            var resultsAsDTO = _bl.ReadAll();
            var resultsAsModel = ClientCarModel.FromDtos(resultsAsDTO);
            return View(resultsAsModel);
        }

        public ActionResult Details(int id)
        {
            return GetRecordById(id);
        }

        public ActionResult Create(int clientId)
        {
            var activeCarBrands = _carBrandBl.ReadActive();
            var activeCarBrandsAsModel = CarBrandModel.FromDtos(activeCarBrands);
            var carBrandOptions = new SelectList(activeCarBrandsAsModel, nameof(CarBrandModel.Id), nameof(CarBrandModel.BrandName));

            var model = new ClientCarModel
            {
                ClientId = clientId,
                CarBrandId = activeCarBrands.First().Id,
                CarBrandOptions = carBrandOptions,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                await _bl.CreateAsync(new ClientCarDTO
                {
                    ClientId = int.Parse(collection["ClientId"]),
                    CarBrandId = int.Parse(collection["CarBrandId"]),
                    LicensePlate = collection["LicensePlate"],
                    Mileage = int.Parse(collection["Mileage"]),
                });
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Create(clientId: int.Parse(collection["ClientId"]));
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

        public ActionResult ClientCars(int clientId)
        {
            var resultsAsDTO = _bl.ReadForClientId(clientId);
            var resultsAsModel = ClientCarModel.FromDtos(resultsAsDTO);
            return View("Index", resultsAsModel);
        }

        private ActionResult GetRecordById(int id)
        {
            var activeCarBrands = _carBrandBl.ReadActive();
            var activeCarBrandsAsModel = CarBrandModel.FromDtos(activeCarBrands);
            var carBrandOptions = new SelectList(activeCarBrandsAsModel, nameof(CarBrandModel.Id), nameof(CarBrandModel.BrandName));

            var activeClients = _clientBl.ReadActive();
            var activeClientsAsModel = ClientModel.FromDtos(activeClients);
            var clientOptions = new SelectList(activeClientsAsModel, nameof(ClientModel.Id), nameof(ClientModel.FullName));

            var resultAsDTO = _bl.ReadById(id);
            var resultAsModel = ClientCarModel.FromDto(resultAsDTO);
            resultAsModel.ClientOptions = clientOptions;
            resultAsModel.CarBrandOptions = carBrandOptions;
            return View(resultAsModel);
        }
    }
}
