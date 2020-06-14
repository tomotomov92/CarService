using BusinessLogic.BLs;
using BusinessLogic.DTOs;
using CarService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CarService.Controllers
{
    public class InspectionController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBaseBL<InspectionDTO> _bl;

        public InspectionController(ILogger<HomeController> logger, IBaseBL<InspectionDTO> bl)
        {
            _logger = logger;
            _bl = bl;
        }

        // GET: InspectionController
        public async Task<ActionResult> Index()
        {
            var resultsAsDTO = await _bl.GetAllAsync();
            var resultsAsModel = InspectionModel.FromDtos(resultsAsDTO);
            return View("InspectionListView", resultsAsModel);
        }

        // GET: InspectionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: InspectionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InspectionController/Create
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

        // GET: InspectionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InspectionController/Edit/5
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

        // GET: InspectionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InspectionController/Delete/5
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
