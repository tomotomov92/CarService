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
            if (_userRole == UserRoles.Owner)
            {
                var resultsAsDTO = _bl.ReadAll();
                var resultsAsModel = CarBrandModel.FromDtos(resultsAsDTO);
                return View(resultsAsModel);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public ActionResult Details(int id)
        {
            return GetRecordById(id);
        }

        public ActionResult Create()
        {
            if (_userRole == UserRoles.Owner)
            {
                return View();
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
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
                return Create();
            }
        }

        public ActionResult Edit(int id)
        {
            if (_userRole == UserRoles.Owner)
            {
                return GetRecordById(id);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
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
             if (_userRole == UserRoles.Owner)
             {
                 return GetRecordById(id);
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
                return GetRecordById(id);
            }
        }

        private ActionResult GetRecordById(int id)
        {
            var resultAsDTO = _bl.ReadById(id);
            var resultAsModel = CarBrandModel.FromDto(resultAsDTO);
            return View(resultAsModel);
        }
    }
}
