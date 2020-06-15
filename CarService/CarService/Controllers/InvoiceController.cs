using BusinessLogic.BLs;
using BusinessLogic.DTOs;
using CarService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CarService.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly IBaseBL<InvoiceDTO> _bl;

        public InvoiceController(ILogger<InvoiceController> logger, IBaseBL<InvoiceDTO> bl)
        {
            _logger = logger;
            _bl = bl;
        }

        public async Task<ActionResult> Index()
        {
            var resultsAsDTO = await _bl.GetAllAsync();
            var resultsAsModel = InvoiceModel.FromDtos(resultsAsDTO);
            return View(resultsAsModel);
        }

        public ActionResult Details(int id)
        {
            return GetRecordById(id);
        }

        public ActionResult Create(int inspectionId)
        {
            var model = new InvoiceModel
            {
                InspectionId = inspectionId,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                await _bl.AddAsync(new InvoiceDTO
                {
                    InspectionId = int.Parse(collection["InspectionId"]),
                    InvoiceDate = DateTime.Parse(collection["InvoiceDate"]),
                    InvoiceSum = decimal.Parse(collection["InvoiceSum"]),
                    Description = collection["Description"],
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
                await _bl.UpdateAsync(new InvoiceDTO
                {
                    Id = id,
                    InspectionId = int.Parse(collection["InspectionId"]),
                    InvoiceDate = DateTime.Parse(collection["DateTimeOfInspection"]),
                    InvoiceSum = decimal.Parse(collection["Description"]),
                    Description = collection["Description"],
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
            var resultAsModel = InvoiceModel.FromDto(resultAsDTO);
            return View(resultAsModel);
        }
    }
}
