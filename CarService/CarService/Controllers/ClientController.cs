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
    public class ClientController : Controller
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IBaseBL<ClientDTO> _bl;

        public ClientController(IHttpContextAccessor httpContextAccessor, ILogger<ClientController> logger, ICredentialBL<ClientDTO> bl)
        {
            _logger = logger;
            _bl = bl;
        }

        public ActionResult Index()
        {
            var userRoleValue = HttpContext.Session.GetInt32(Constants.SessionKeyUserRole);
            if (userRoleValue != null)
            {
                var userRole = (UserRoles)userRoleValue;
                if (userRole == UserRoles.Owner ||
                    userRole == UserRoles.CustomerSupport)
                {
                    var resultsAsDTO = _bl.ReadAll();
                    var resultsAsModel = ClientModel.FromDtos(resultsAsDTO);
                    return View(resultsAsModel);
                }
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public ActionResult Details(int id)
        {
            var userRoleValue = HttpContext.Session.GetInt32(Constants.SessionKeyUserRole);
            if (userRoleValue != null)
            {
                var userRole = (UserRoles)userRoleValue;
                if (userRole == UserRoles.Owner ||
                    userRole == UserRoles.CustomerSupport)
                {
                    return GetActionForRecordById(id);
                }
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                await _bl.CreateAsync(new ClientDTO
                {
                    FirstName = collection["FirstName"],
                    LastName = collection["LastName"],
                    EmailAddress = collection["EmailAddress"],
                    Password = Constants.DefaultPassword,
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
            var userRoleValue = HttpContext.Session.GetInt32(Constants.SessionKeyUserRole);
            if (userRoleValue != null)
            {
                var userRole = (UserRoles)userRoleValue;
                if (userRole == UserRoles.Owner ||
                    userRole == UserRoles.CustomerSupport)
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
                await _bl.UpdateAsync(new ClientDTO
                {
                    Id = id,
                    FirstName = collection["FirstName"],
                    LastName = collection["LastName"],
                    EmailAddress = collection["EmailAddress"],
                    Password = collection["Password"],
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
            var userRoleValue = HttpContext.Session.GetInt32(Constants.SessionKeyUserRole);
            if (userRoleValue != null)
            {
                var userRole = (UserRoles)userRoleValue;
                if (userRole == UserRoles.Owner ||
                    userRole == UserRoles.CustomerSupport)
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

        private ClientDTO GetRecordById(int id)
        {
            return _bl.ReadById(id);
        }

        public ActionResult GetActionForRecordById(int id)
        {
            var resultAsDTO = GetRecordById(id);
            var resultAsModel = ClientModel.FromDto(resultAsDTO);
            return View(resultAsModel);
        }
    }
}
