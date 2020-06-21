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
    public class InvoiceController : Controller
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly IInvoiceBL<InvoiceDTO> _bl;

        public InvoiceController(IHttpContextAccessor httpContextAccessor, ILogger<InvoiceController> logger, IInvoiceBL<InvoiceDTO> bl)
        {
            _logger = logger;
            _bl = bl;
        }

        public ActionResult Index()
        {
            var resultsAsDTO = _bl.ReadAll();
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
                var dateForInvoice = DateTime.Parse(collection["DateForInvoice"]);
                var timeForInvoice = TimeSpan.Parse(collection["TimeForInvoice"]);

                var invoiceDate = dateForInvoice + timeForInvoice;

                await _bl.CreateAsync(new InvoiceDTO
                {
                    InspectionId = int.Parse(collection["InspectionId"]),
                    InvoiceDate = invoiceDate,
                    InvoiceSum = decimal.Parse(collection["InvoiceSum"]),
                    Description = collection["Description"],
                });
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Create(inspectionId: int.Parse(collection["InspectionId"]));
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
                var dateForInvoice = DateTime.Parse(collection["DateForInvoice"]);
                var timeForInvoice = TimeSpan.Parse(collection["TimeForInvoice"]);

                var invoiceDate = dateForInvoice + timeForInvoice;

                await _bl.UpdateAsync(new InvoiceDTO
                {
                    Id = id,
                    InspectionId = int.Parse(collection["InspectionId"]),
                    InvoiceDate = invoiceDate,
                    InvoiceSum = decimal.Parse(collection["InvoiceSum"]),
                    Description = collection["Description"],
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
            catch (Exception ex)
            {
                return GetRecordById(id);
            }
        }

        public ActionResult ClientInvoices(int clientId)
        {
            var resultsAsDTO = _bl.ReadForClientId(clientId);
            var resultsAsModel = InvoiceModel.FromDtos(resultsAsDTO);
            return View("Index", resultsAsModel);
        }

        private ActionResult GetRecordById(int id)
        {
            var resultAsDTO = _bl.ReadById(id);
            var resultAsModel = InvoiceModel.FromDto(resultAsDTO);
            return View(resultAsModel);
        }
    }
}
