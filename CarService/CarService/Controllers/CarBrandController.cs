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
    public class CarBrandController : Controller
    {
        private readonly ILogger<CarBrandController> _logger;
        private readonly IBaseBL<CarBrandDTO> _bl;
        private readonly UserRoles _userRole = UserRoles.NA;

        public CarBrandController(IHttpContextAccessor httpContextAccessor, ILogger<CarBrandController> logger, IBaseBL<CarBrandDTO> bl)
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
            switch (_userRole)
            {
                case UserRoles.Owner:
                    {
                        var resultsAsDTO = _bl.ReadAll();
                        var resultsAsModel = CarBrandModel.FromDtos(resultsAsDTO);
                        return View(resultsAsModel);
                    }
                default:
                    return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        public ActionResult Details(int id)
        {
            return GetActionForRecordById(id);
        }

        public ActionResult Create()
        {
            switch (_userRole)
            {
                case UserRoles.Owner:
                    return View();
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
                await _bl.CreateAsync(new CarBrandDTO
                {
                    BrandName = collection["BrandName"],
                });
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CarBrandController.Create));
                return Create();
            }
        }

        public ActionResult Edit(int id)
        {
            switch (_userRole)
            {
                case UserRoles.Owner:
                    return GetActionForRecordById(id);
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
                await _bl.UpdateAsync(new CarBrandDTO
                {
                    Id = id,
                    BrandName = collection["BrandName"],
                });
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CarBrandController.Edit));
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
                        {
                            await _bl.ArchiveAsync(new CarBrandDTO
                            {
                                Id = id,
                                Archived = true,
                            });
                            return RedirectToAction(nameof(Index));
                        }
                    default:
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CarBrandController.Archive));
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
                    {
                        await _bl.ArchiveAsync(new CarBrandDTO
                        {
                            Id = id,
                            Archived = false,
                        });
                        return RedirectToAction(nameof(Index));
                    }
                default:
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CarBrandController.Unarchive));
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Delete(int id)
        {
            switch (_userRole)
            {
                case UserRoles.Owner:
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
                _logger.LogError(ex, nameof(CarBrandController.Delete));
                return GetActionForRecordById(id);
            }
        }

        private ActionResult GetActionForRecordById(int id)
        {
            var resultAsDTO = _bl.ReadById(id);
            var resultAsModel = CarBrandModel.FromDto(resultAsDTO);
            return View(resultAsModel);
        }
    }
}
