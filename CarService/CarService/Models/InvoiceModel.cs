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

        public int InvoiceId { get; set; }

        public DateTime InvoiceDate { get; set; }

        public decimal InvoiceSum { get; set; }

        public string Description { get; set; }

        public bool Archived { get; set; }

        public static InvoiceModel FromDto(InvoiceDTO dto)
        {
            return new InvoiceModel
            {
                Id = dto.Id,
                InvoiceId = dto.InvoiceId,
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
                InvoiceId = dto.InvoiceId,
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
                InvoiceId = model.InvoiceId,
                InvoiceSum = model.InvoiceSum,
                InvoiceDate = model.InvoiceDate,
                Description = model.Description,
                Archived = model.Archived,
            };
        }
    }
}