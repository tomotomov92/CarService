using BusinessLogic;
using BusinessLogic.BLs.Interfaces;
using BusinessLogic.DTOs;
using BusinessLogic.Enums;
using CarService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
            var userId = httpContextAccessor.HttpContext.Session.GetInt32(Constants.SessionKeyUserId);
            if (userId != null)
            {
                _userId = userId.Value;
            }
        }

        public ActionResult Index()
        {
            IEnumerable<InspectionDTO> resultsAsDTO = null;

            switch (_userRole)
            {
                case UserRoles.Owner:
                case UserRoles.CustomerSupport:
                    resultsAsDTO = _bl.ReadAll();
                    break;
                case UserRoles.Mechanic:
                    resultsAsDTO = _bl.ReadForEmployeeId(_userId);
                    break;
                case UserRoles.Customer:
                    resultsAsDTO = _bl.ReadForClientId(_userId);
                    break;
                default:
                    return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var resultsAsModel = InspectionModel.FromDtos(resultsAsDTO);
            return View("Index", resultsAsModel);
        }

        public ActionResult Details(int id)
        {
            return GetActionForRecordById(id);
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
                _logger.LogError(ex, nameof(EmployeeController.Create));
                return Create(clientId: int.Parse(collection["ClientId"]), carId: int.Parse(collection["CarId"]));
            }
        }

        public ActionResult Edit(int id)
        {
            return GetActionForRecordById(id);
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
                });

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(EmployeeController.Edit));
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
                            await _bl.ArchiveAsync(new InspectionDTO
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
                                await _bl.ArchiveAsync(new InspectionDTO
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
                _logger.LogError(ex, nameof(InspectionController.Archive));
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
                            await _bl.ArchiveAsync(new InspectionDTO
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
                                await _bl.ArchiveAsync(new InspectionDTO
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
                _logger.LogError(ex, nameof(InspectionController.Unarchive));
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Delete(int id)
        {
            return GetActionForRecordById(id);
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

        public ActionResult ClientInspections(int clientId)
        {
            try
            {
                switch (_userRole)
                {
                    case UserRoles.Owner:
                    case UserRoles.CustomerSupport:
                    case UserRoles.Customer:
                        {
                            var resultsAsDTO = _bl.ReadForClientId(clientId);
                            var resultsAsModel = InspectionModel.FromDtos(resultsAsDTO);
                            return View("Index", resultsAsModel);
                        }
                    default:
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(InspectionController.ClientInspections));
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult CarInspections(int carId)
        {
            try
            {
                switch (_userRole)
                {
                    case UserRoles.Owner:
                    case UserRoles.CustomerSupport:
                    case UserRoles.Customer:
                        {
                            var resultsAsDTO = _bl.ReadForCarId(carId);
                            var resultsAsModel = InspectionModel.FromDtos(resultsAsDTO);
                            return View("Index", resultsAsModel);
                        }
                    default:
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(InspectionController.CarInspections));
                return RedirectToAction(nameof(Index));
            }
        }

        private InspectionDTO GetRecordById(int id)
        {
            return _bl.ReadById(id);
        }

        private ActionResult GetActionForRecordById(int id)
        {
            var resultAsDTO = GetRecordById(id);
            var resultAsModel = InspectionModel.FromDto(resultAsDTO);
            return View(resultAsModel);
        }
    }
}
