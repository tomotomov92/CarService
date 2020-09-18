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
    public class InvoiceController : Controller
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly IInvoiceBL<InvoiceDTO> _bl;
        private readonly IClientCarBL<CarDTO> _clientCarBl;
        private readonly IInspectionBL<InspectionDTO> _inspectionBl;
        private readonly UserRoles _userRole = UserRoles.NA;
        private readonly int _userId = 0;

        public InvoiceController(IHttpContextAccessor httpContextAccessor, ILogger<InvoiceController> logger, IInvoiceBL<InvoiceDTO> bl, IClientCarBL<CarDTO> clientCarBl, IInspectionBL<InspectionDTO> inspectionBl)
        {
            _logger = logger;
            _bl = bl;
            _clientCarBl = clientCarBl;
            _inspectionBl = inspectionBl;

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
            IEnumerable<InvoiceDTO> resultsAsDTO = null;

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

            var resultsAsModel = InvoiceModel.FromDtos(resultsAsDTO);
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
                        if (record.Inspection.ClientId == _userId)
                        {
                            return GetActionForRecordById(id);
                        }
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                default:
                    return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        public ActionResult Create(int inspectionId)
        {
            switch (_userRole)
            {
                case UserRoles.Owner:
                case UserRoles.CustomerSupport:
                    {
                        var model = new InvoiceModel
                        {
                            InspectionId = inspectionId,
                        };
                        return View(model);
                    }
                case UserRoles.Customer:
                    {
                        var record = GetInspectionRecordById(inspectionId);
                        if (record.ClientId == _userId)
                        {
                            var model = new InvoiceModel
                            {
                                InspectionId = inspectionId,
                            };
                            return View(model);
                        }
                        return RedirectToAction(nameof(HomeController.Index), "Home");
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
                var dateForInvoice = DateTime.Parse(collection["DateForInvoice"]);
                var timeForInvoice = TimeSpan.Parse(collection["TimeForInvoice"]);

                var invoiceDate = dateForInvoice + timeForInvoice;

                await _bl.CreateAsync(new InvoiceDTO
                {
                    InspectionId = int.Parse(collection["InspectionId"]),
                    InvoiceDate = invoiceDate,
                    InvoiceSum = decimal.Parse(collection["InvoiceSum"]),
                    Description = collection["Description"],
                });
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(EmployeeController.Create));
                return Create(inspectionId: int.Parse(collection["InspectionId"]));
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
                        if (record.Inspection.ClientId == _userId)
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
                var dateForInvoice = DateTime.Parse(collection["DateForInvoice"]);
                var timeForInvoice = TimeSpan.Parse(collection["TimeForInvoice"]);

                var invoiceDate = dateForInvoice + timeForInvoice;

                await _bl.UpdateAsync(new InvoiceDTO
                {
                    Id = id,
                    InspectionId = int.Parse(collection["InspectionId"]),
                    InvoiceDate = invoiceDate,
                    InvoiceSum = decimal.Parse(collection["InvoiceSum"]),
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
                            await _bl.ArchiveAsync(new InvoiceDTO
                            {
                                Id = id,
                                Archived = true,
                            });
                            return RedirectToAction(nameof(Index));
                        }
                    case UserRoles.Customer:
                        {
                            var record = GetRecordById(id);
                            if (record.Inspection.ClientId == _userId)
                            {
                                await _bl.ArchiveAsync(new InvoiceDTO
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
                _logger.LogError(ex, nameof(InvoiceController.Archive));
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
                            await _bl.ArchiveAsync(new InvoiceDTO
                            {
                                Id = id,
                                Archived = false,
                            });
                            return RedirectToAction(nameof(Index));
                        }
                    case UserRoles.Customer:
                        {
                            var record = GetRecordById(id);
                            if (record.Inspection.ClientId == _userId)
                            {
                                await _bl.ArchiveAsync(new InvoiceDTO
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
                _logger.LogError(ex, nameof(InvoiceController.Unarchive));
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

        public ActionResult ClientInvoices(int clientId)
        {
            try
            {
                switch (_userRole)
                {
                    case UserRoles.Owner:
                    case UserRoles.CustomerSupport:
                        {
                            var resultsAsDTO = _bl.ReadForClientId(clientId);
                            var resultsAsModel = InvoiceModel.FromDtos(resultsAsDTO);
                            return View("Index", resultsAsModel);
                        }
                    case UserRoles.Customer:
                        {
                            if (_userId == clientId)
                            {
                                var resultsAsDTO = _bl.ReadForClientId(clientId);
                                var resultsAsModel = InvoiceModel.FromDtos(resultsAsDTO);
                                return View("Index", resultsAsModel);
                            }
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    default:
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(InvoiceController.ClientInvoices));
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult CarInvoices(int carId)
        {
            try
            {
                switch (_userRole)
                {
                    case UserRoles.Owner:
                    case UserRoles.CustomerSupport:
                        {
                            var resultsAsDTO = _bl.ReadForCarId(carId);
                            var resultsAsModel = InvoiceModel.FromDtos(resultsAsDTO);
                            return View("Index", resultsAsModel);
                        }
                    case UserRoles.Customer:
                        {
                            var record = GetCarRecordById(carId);
                            if (record.ClientId == _userId)
                            {
                                var resultsAsDTO = _bl.ReadForCarId(carId);
                                var resultsAsModel = InvoiceModel.FromDtos(resultsAsDTO);
                                return View("Index", resultsAsModel);
                            }
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    default:
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(InvoiceController.CarInvoices));
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult InspectionInvoices(int inspectionId)
        {
            try
            {
                switch (_userRole)
                {
                    case UserRoles.Owner:
                    case UserRoles.CustomerSupport:
                        {
                            var resultsAsDTO = _bl.ReadForInspectionId(inspectionId);
                            var resultsAsModel = InvoiceModel.FromDtos(resultsAsDTO);
                            return View("Index", resultsAsModel);
                        }
                    case UserRoles.Customer:
                        {
                            var record = GetInspectionRecordById(inspectionId);
                            if (record.ClientId == _userId)
                            {
                                var resultsAsDTO = _bl.ReadForInspectionId(inspectionId);
                                var resultsAsModel = InvoiceModel.FromDtos(resultsAsDTO);
                                return View("Index", resultsAsModel);
                            }
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    default:
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(InvoiceController.InspectionInvoices));
                return RedirectToAction(nameof(Index));
            }
        }

        private InvoiceDTO GetRecordById(int id)
        {
            return _bl.ReadById(id);
        }

        private InspectionDTO GetInspectionRecordById(int id)
        {
            return _inspectionBl.ReadById(id);
        }

        private CarDTO GetCarRecordById(int id)
        {
            return _clientCarBl.ReadById(id);
        }

        private ActionResult GetActionForRecordById(int id)
        {
            var resultAsDTO = GetRecordById(id);
            var resultAsModel = InvoiceModel.FromDto(resultAsDTO);
            return View(resultAsModel);
        }
    }
}
