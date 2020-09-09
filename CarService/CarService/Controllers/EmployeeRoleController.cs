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
    public class EmployeeRoleController : Controller
    {
        private readonly ILogger<EmployeeRoleController> _logger;
        private readonly IBaseBL<EmployeeRoleDTO> _bl;
        private readonly UserRoles _userRole = UserRoles.NA;

        public EmployeeRoleController(IHttpContextAccessor httpContextAccessor, ILogger<EmployeeRoleController> logger, IBaseBL<EmployeeRoleDTO> bl)
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
                        var resultsAsModel = EmployeeRoleModel.FromDtos(resultsAsDTO);
                        return View(resultsAsModel);
                    }
                default:
                    return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        public ActionResult Details(int id)
        {
            switch (_userRole)
            {
                case UserRoles.Owner:
                    return GetActionForRecordById(id);
                default:
                    return RedirectToAction(nameof(HomeController.Index), "Home");
            }
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
                await _bl.CreateAsync(new EmployeeRoleDTO
                {
                    EmployeeRoleName = collection["EmployeeRoleName"],
                });
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(EmployeeController.Create));
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
                await _bl.UpdateAsync(new EmployeeRoleDTO
                {
                    Id = id,
                    EmployeeRoleName = collection["EmployeeRoleName"],
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
                        {
                            await _bl.ArchiveAsync(new EmployeeRoleDTO
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
                _logger.LogError(ex, nameof(EmployeeRoleController.Archive));
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
                            await _bl.ArchiveAsync(new EmployeeRoleDTO
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
                _logger.LogError(ex, nameof(EmployeeRoleController.Unarchive));
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
                _logger.LogError(ex, nameof(EmployeeController.Delete));
                return GetActionForRecordById(id);
            }
        }

        private ActionResult GetActionForRecordById(int id)
        {
            var resultAsDTO = _bl.ReadById(id);
            var resultAsModel = EmployeeRoleModel.FromDto(resultAsDTO);
            return View(resultAsModel);
        }
    }
}
