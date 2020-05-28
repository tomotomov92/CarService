using BusinessLogic.BLs;
using BusinessLogic.DTOs;
using CarService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CarService.Controllers
{
    public class CarBrandController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBaseBL<CarBrandDTO> _bl;

        public CarBrandController(ILogger<HomeController> logger, IBaseBL<CarBrandDTO> bl)
        {
            _logger = logger;
            _bl = bl;
        }

        // GET: CarBrandController
        public async Task<ActionResult> Index()
        {
            var resultsAsDTO = await _bl.GetAllAsync();
            var resultsAsModel = CarBrandModel.FromDtos(resultsAsDTO);
            return View("CarBrandListView", resultsAsModel);
        }

        // GET: CarBrandController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CarBrandController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CarBrandController/Create
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

        // GET: CarBrandController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CarBrandController/Edit/5
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

        // GET: CarBrandController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CarBrandController/Delete/5
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
