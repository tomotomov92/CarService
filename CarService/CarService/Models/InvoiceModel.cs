using BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CarService.Models
{
    public class InvoiceModel
    {
        public int Id { get; set; }

        public int InspectionId { get; set; }

        public InspectionModel Inspection { get; set; }

        public DateTime InvoiceDate { get; set; }

        public decimal InvoiceSum { get; set; }

        public string Description { get; set; }

        public bool Archived { get; set; }

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