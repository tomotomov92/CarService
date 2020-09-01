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
using System.Collections.Generic;
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
        private readonly UserRoles _userRole = UserRoles.NA;
        private readonly int _userId = 0;

        public ClientCarController(IHttpContextAccessor httpContextAccessor, ILogger<ClientCarController> logger, IClientCarBL<ClientCarDTO> bl, IBaseBL<CarBrandDTO> carBrandBl, ICredentialBL<ClientDTO> clientBl)
        {
            _logger = logger;
            _bl = bl;
            _carBrandBl = carBrandBl;
            _clientBl = clientBl;

            var userRoleInt = httpContextAccessor.HttpContext.Session.GetInt32(Constants.SessionKeyUserRole);
            if (userRoleInt != null)
            {
                _userRole = (UserRoles)userRoleInt;
            }
            _userId = httpContextAccessor.HttpContext.Session.GetInt32(Constants.SessionKeyUserId).Value;
        }

        public ActionResult Index()
        {
            IEnumerable<ClientCarDTO> resultsAsDTO = null;

            switch (_userRole)
            {
                case UserRoles.Owner:
                case UserRoles.CustomerSupport:
                    resultsAsDTO = _bl.ReadAll();
                    break;
                case UserRoles.Customer:
                    resultsAsDTO = _bl.ReadForClientId(_userId);
                    break;
                default:
                    return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var resultsAsModel = ClientCarModel.FromDtos(resultsAsDTO);
            return View("Index", resultsAsModel);
        }

        public ActionResult Details(int id)
        {
            if (_userRole == UserRoles.Owner ||
                _userRole == UserRoles.CustomerSupport)
            {
                return GetActionForRecordById(id);
            }
            else if (_userRole == UserRoles.Customer)
            {
                var record = GetRecordById(id);
                if (record.ClientId == _userId)
                {
                    return GetActionForRecordById(id);
                }
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public ActionResult Create(int clientId)
        {
            if (_userRole == UserRoles.Owner ||
                _userRole == UserRoles.CustomerSupport ||
                _userRole == UserRoles.Customer)
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
            return RedirectToAction(nameof(HomeController.Index), "Home");
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
            if (_userRole == UserRoles.Owner ||
                _userRole == UserRoles.CustomerSupport)
            {
                return GetActionForRecordById(id);
            }
            else if (_userRole == UserRoles.Customer)
            {
                var record = GetRecordById(id);
                if (record.ClientId == _userId)
                {
                    return GetActionForRecordById(id);
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
                return GetActionForRecordById(id);
            }
        }

        public ActionResult Delete(int id)
        {
            if (_userRole == UserRoles.Owner ||
                _userRole == UserRoles.CustomerSupport)
            {
                return GetActionForRecordById(id);
            }
            else if (_userRole == UserRoles.Customer)
            {
                var record = GetRecordById(id);
                if (record.ClientId == _userId)
                {
                    return GetActionForRecordById(id);
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
                return GetActionForRecordById(id);
            }
        }

        private ClientCarDTO GetRecordById(int id)
        {
            return _bl.ReadById(id);
        }

        public ActionResult GetActionForRecordById(int id)
        {
            var activeCarBrands = _carBrandBl.ReadActive();
            var activeCarBrandsAsModel = CarBrandModel.FromDtos(activeCarBrands);
            var carBrandOptions = new SelectList(activeCarBrandsAsModel, nameof(CarBrandModel.Id), nameof(CarBrandModel.BrandName));

            var activeClients = _clientBl.ReadActive();
            var activeClientsAsModel = ClientModel.FromDtos(activeClients);
            var clientOptions = new SelectList(activeClientsAsModel, nameof(ClientModel.Id), nameof(ClientModel.FullName));

            var resultAsDTO = GetRecordById(id);
            var resultAsModel = ClientCarModel.FromDto(resultAsDTO);
            resultAsModel.ClientOptions = clientOptions;
            resultAsModel.CarBrandOptions = carBrandOptions;
            return View(resultAsModel);
        }
    }
}
