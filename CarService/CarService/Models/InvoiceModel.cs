using BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CarService.Models
{
    public class InvoiceModel
    {
        public int Id { get; set; }

        [Required]
        public int InspectionId { get; set; }

        public InspectionModel Inspection { get; set; }

        [Required, DisplayName("Invoice Date"), DataType(DataType.DateTime)]
        public DateTime InvoiceDate { get; set; }

        [Required, DisplayName("Invoice Sum")]
        public decimal InvoiceSum { get; set; }

        public string Description { get; set; }

        public bool Archived { get; set; }

        [Required, DisplayName("Invoice Date"), DataType(DataType.Date)]
        public DateTime DateForInvoice { get; set; } = DateTime.Now;

        [Required, DisplayName("Invoice Time"), DataType(DataType.Time)]
        public TimeSpan TimeForInvoice { get; set; } = TimeSpan.Parse("09:00:00");

        public static InvoiceModel FromDto(InvoiceDTO dto)
        {
            return new InvoiceModel
            {
                Id = dto.Id,
                InspectionId = dto.InspectionId,
                Inspection = InspectionModel.FromDto(dto.Inspection),
                InvoiceSum = dto.InvoiceSum,
                InvoiceDate = dto.InvoiceDate,
                Description = dto.Description,
                Archived = dto.Archived,
                DateForInvoice = dto.InvoiceDate.Date,
                TimeForInvoice = dto.InvoiceDate.TimeOfDay,
            };
        }

        public static IEnumerable<InvoiceModel> FromDtos(IEnumerable<InvoiceDTO> dtos)
        {
            return dtos.Select(dto => new InvoiceModel
            {
                Id = dto.Id,
                InspectionId = dto.InspectionId,
                Inspection = InspectionModel.FromDto(dto.Inspection),
                InvoiceSum = dto.InvoiceSum,
                InvoiceDate = dto.InvoiceDate,
                Description = dto.Description,
                Archived = dto.Archived,
                DateForInvoice = dto.InvoiceDate.Date,
                TimeForInvoice = dto.InvoiceDate.TimeOfDay,
            });
        }

        public static InvoiceDTO ToDto(InvoiceModel model)
        {
            return new InvoiceDTO
            {
                Id = model.Id,
                InspectionId = model.InspectionId,
                Inspection = InspectionModel.ToDto(model.Inspection),
                InvoiceSum = model.InvoiceSum,
                InvoiceDate = model.InvoiceDate,
                Description = model.Description,
                Archived = model.Archived,
            };
        }
    }
}