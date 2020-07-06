using BusinessLogic.BLs.Interfaces;
using BusinessLogic.DTOs;
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

        public EmployeeRoleController(IHttpContextAccessor httpContextAccessor, ILogger<EmployeeRoleController> logger, IBaseBL<EmployeeRoleDTO> bl)
        {
            _logger = logger;
            _bl = bl;
        }

        public ActionResult Index()
        {
            var resultsAsDTO = _bl.ReadAll();
            var resultsAsModel = EmployeeRoleModel.FromDtos(resultsAsDTO);
            return View(resultsAsModel);
        }

        public ActionResult Details(int id)
        {
            return GetRecordById(id);
        }

        //public ActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        await _bl.CreateAsync(new EmployeeRoleDTO
        //        {
        //            EmployeeRoleName = collection["EmployeeRoleName"],
        //        });
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Create();
        //    }
        //}

        //public ActionResult Edit(int id)
        //{
        //    return GetRecordById(id);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        await _bl.UpdateAsync(new EmployeeRoleDTO
        //        {
        //            Id = id,
        //            EmployeeRoleName = collection["EmployeeRoleName"],
        //            Archived = bool.Parse(collection["Archived"][0]),
        //        });
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        return GetRecordById(id);
        //    }
        //}

        //public ActionResult Delete(int id)
        //{
        //    return GetRecordById(id);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        await _bl.DeleteAsync(id);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        return GetRecordById(id);
        //    }
        //}

        private ActionResult GetRecordById(int id)
        {
            var resultAsDTO = _bl.ReadById(id);
            var resultAsModel = EmployeeRoleModel.FromDto(resultAsDTO);
            return View(resultAsModel);
        }
    }
}
