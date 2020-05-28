using BusinessLogic.BLs;
using BusinessLogic.DTOs;
using CarService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CarService.Controllers
{
    public class InspectionHoursController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBaseBL<InspectionHoursDTO> _bl;

        public InspectionHoursController(ILogger<HomeController> logger, IBaseBL<InspectionHoursDTO> bl)
        {
            _logger = logger;
            _bl = bl;
        }

        // GET: InspectionHoursController
        public async Task<ActionResult> Index()
        {
            var resultsAsDTO = await _bl.GetAllAsync();
            var resultsAsModel = InspectionHoursModel.FromDtos(resultsAsDTO);
            return View("InspectionHoursListView", resultsAsModel);
        }

        // GET: InspectionHoursController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: InspectionHoursController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InspectionHoursController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InspectionHoursController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InspectionHoursController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InspectionHoursController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InspectionHoursController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
