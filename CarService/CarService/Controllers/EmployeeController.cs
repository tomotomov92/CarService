using BusinessLogic.BLs;
using BusinessLogic.DTOs;
using CarService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CarService.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IBaseBL<EmployeeDTO> _bl;
        private readonly IBaseBL<EmployeeRoleDTO> _employeeRoleBl;

        public EmployeeController(ILogger<EmployeeController> logger, IBaseBL<EmployeeDTO> bl, IBaseBL<EmployeeRoleDTO> employeeRoleBl)
        {
            _logger = logger;
            _bl = bl;
            _employeeRoleBl = employeeRoleBl;
        }

        public async Task<ActionResult> Index()
        {
            var resultsAsDTO = await _bl.GetAllAsync();
            var resultsAsModel = EmployeeModel.FromDtos(resultsAsDTO);
            return View(resultsAsModel);
        }

        public ActionResult Details(int id)
        {
            return GetRecordById(id);
        }

        public async Task<ActionResult> Create()
        {
            var activeEmployeeRoles = await _employeeRoleBl.GetAllActiveAsync();
            var employeeRolesOptions = new SelectList(activeEmployeeRoles, nameof(EmployeeRoleModel.Id), nameof(EmployeeRoleModel.EmployeeRoleName));
            var model = new EmployeeModel
            {
                EmployeeRoleOptions = employeeRolesOptions,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                await _bl.AddAsync(new EmployeeDTO
                {
                    FirstName = collection["FirstName"],
                    LastName = collection["LastName"],
                    EmailAddress = collection["EmailAddress"],
                    Password = collection["Password"],
                    DateOfStart = DateTime.Parse(collection["DateOfStart"]),
                    EmployeeRoleId = int.Parse(collection["EmployeeRoleId"]),
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
                await _bl.UpdateAsync(new EmployeeDTO
                {
                    Id = id,
                    FirstName = collection["FirstName"],
                    LastName = collection["LastName"],
                    EmailAddress = collection["EmailAddress"],
                    Password = collection["Password"],
                    DateOfStart = DateTime.Parse(collection["DateOfStart"]),
                    EmployeeRoleId = int.Parse(collection["EmployeeRoleId"]),
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
            var resultAsModel = EmployeeModel.FromDto(resultAsDTO);
            return View(resultAsModel);
        }
    }
}
