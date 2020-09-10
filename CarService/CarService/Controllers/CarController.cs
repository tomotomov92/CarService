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
    public class CarController : Controller
    {
        private readonly ILogger<CarController> _logger;
        private readonly IClientCarBL<CarDTO> _bl;
        private readonly IBaseBL<CarBrandDTO> _carBrandBl;
        private readonly IBaseBL<ClientDTO> _clientBl;
        private readonly UserRoles _userRole = UserRoles.NA;
        private readonly int _userId = 0;

        public CarController(IHttpContextAccessor httpContextAccessor, ILogger<CarController> logger, IClientCarBL<CarDTO> bl, IBaseBL<CarBrandDTO> carBrandBl, ICredentialBL<ClientDTO> clientBl)
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
            var userId = httpContextAccessor.HttpContext.Session.GetInt32(Constants.SessionKeyUserId);
            if (userId != null)
            {
                _userId = userId.Value;
            }
        }

        public ActionResult Index()
        {
            IEnumerable<CarDTO> resultsAsDTO = null;

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

            var resultsAsModel = CarModel.FromDtos(resultsAsDTO);
            return View("Index", resultsAsModel);
        }

        public ActionResult Details(int id)
        {
            switch (_userRole)
            {
                case UserRoles.Owner:
                case UserRoles.CustomerSupport:
                    return GetActionForRecordById(id);
                case UserRoles.Customer:
                    {
                        var record = GetRecordById(id);
                        if (record.ClientId == _userId)
                        {
                            return GetActionForRecordById(id);
                        }
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                default:
                    return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        public ActionResult Create(int clientId)
        {
            switch (_userRole)
            {
                case UserRoles.Owner:
                case UserRoles.CustomerSupport:
                case UserRoles.Customer:
                    {
                        var activeCarBrands = _carBrandBl.ReadActive();
                        var activeCarBrandsAsModel = CarBrandModel.FromDtos(activeCarBrands);
                        var carBrandOptions = new SelectList(activeCarBrandsAsModel, nameof(CarBrandModel.Id), nameof(CarBrandModel.BrandName));

                        var model = new CarModel
                        {
                            ClientId = clientId,
                            CarBrandId = activeCarBrands.First().Id,
                            CarBrandOptions = carBrandOptions,
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
                await _bl.CreateAsync(new CarDTO
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
                _logger.LogError(ex, nameof(CarController.Create));
                return Create(clientId: int.Parse(collection["ClientId"]));
            }
        }

        public ActionResult Edit(int id)
        {
            switch (_userRole)
            {
                case UserRoles.Owner:
                case UserRoles.CustomerSupport:
                    return GetActionForRecordById(id);
                case UserRoles.Customer:
                    {
                        var record = GetRecordById(id);
                        if (record.ClientId == _userId)
                        {
                            return GetActionForRecordById(id);
                        }
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
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
                await _bl.UpdateAsync(new CarDTO
                {
                    Id = id,
                    ClientId = int.Parse(collection["ClientId"]),
                    CarBrandId = int.Parse(collection["CarBrandId"]),
                    LicensePlate = collection["LicensePlate"],
                    Mileage = int.Parse(collection["Mileage"]),
                });
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CarController.Edit));
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
                    case UserRoles.CustomerSupport:
                        {
                            await _bl.ArchiveAsync(new CarDTO
                            {
                                Id = id,
                                Archived = true,
                            });
                            return RedirectToAction(nameof(Index));
                        }
                    case UserRoles.Customer:
                        {
                            var record = GetRecordById(id);
                            if (record.ClientId == _userId)
                            {
                                await _bl.ArchiveAsync(new CarDTO
                                {
                                    Id = id,
                                    Archived = true,
                                });
                                return RedirectToAction(nameof(Index));
                            }
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    default:
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CarController.Archive));
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
                    case UserRoles.CustomerSupport:
                        {
                            await _bl.ArchiveAsync(new CarDTO
                            {
                                Id = id,
                                Archived = false,
                            });
                            return RedirectToAction(nameof(Index));
                        }
                    case UserRoles.Customer:
                        {
                            var record = GetRecordById(id);
                            if (record.ClientId == _userId)
                            {
                                await _bl.ArchiveAsync(new CarDTO
                                {
                                    Id = id,
                                    Archived = false,
                                });
                                return RedirectToAction(nameof(Index));
                            }
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    default:
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CarController.Unarchive));
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Delete(int id)
        {
            switch (_userRole)
            {
                case UserRoles.Owner:
                case UserRoles.CustomerSupport:
                    return GetActionForRecordById(id);
                case UserRoles.Customer:
                    {
                        var record = GetRecordById(id);
                        if (record.ClientId == _userId)
                        {
                            return GetActionForRecordById(id);
                        }
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
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
                _logger.LogError(ex, nameof(CarController.Delete));
                return GetActionForRecordById(id);
            }
        }

        public ActionResult ClientCars(int clientId)
        {
            try
            {
                switch (_userRole)
                {
                    case UserRoles.Owner:
                    case UserRoles.CustomerSupport:
                        {
                            var resultsAsDTO = _bl.ReadForClientId(clientId);
                            var resultsAsModel = CarModel.FromDtos(resultsAsDTO);
                            return View("Index", resultsAsModel);
                        }
                    default:
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CarController.ClientCars));
                return RedirectToAction(nameof(Index));
            }
        }

        private CarDTO GetRecordById(int id)
        {
            return _bl.ReadById(id);
        }

        private ActionResult GetActionForRecordById(int id)
        {
            var activeCarBrands = _carBrandBl.ReadActive();
            var activeCarBrandsAsModel = CarBrandModel.FromDtos(activeCarBrands);
            var carBrandOptions = new SelectList(activeCarBrandsAsModel, nameof(CarBrandModel.Id), nameof(CarBrandModel.BrandName));

            var activeClients = _clientBl.ReadActive();
            var activeClientsAsModel = ClientModel.FromDtos(activeClients);
            var clientOptions = new SelectList(activeClientsAsModel, nameof(ClientModel.Id), nameof(ClientModel.FullName));

            var resultAsDTO = GetRecordById(id);
            var resultAsModel = CarModel.FromDto(resultAsDTO);
            resultAsModel.ClientOptions = clientOptions;
            resultAsModel.CarBrandOptions = carBrandOptions;
            return View(resultAsModel);
        }
    }
}
