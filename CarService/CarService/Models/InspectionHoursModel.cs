using BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CarService.Models
{
    public class InspectionHoursModel
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public string ClientFirstName { get; set; }

        public string ClientLastName { get; set; }

        [DisplayName("Client Full Name")]
        public string ClientFullName => $"{ClientLastName}, {ClientFirstName}";

        public int CarId { get; set; }

        [DisplayName("Car Brand")]
        public string CarBrandName { get; set; }

        [DisplayName("Inspection Date")]
        public DateTime DateTimeOfInspection { get; set; }

        public static InspectionHoursModel FromDto(InspectionHoursDTO dto)
        {
            return new InspectionHoursModel
            {
                Id = dto.Id,
                ClientId = dto.ClientId,
                ClientFirstName = dto.ClientFirstName,
                ClientLastName = dto.ClientLastName,
                CarId = dto.CarId,
                CarBrandName = dto.CarBrandName,
                DateTimeOfInspection = dto.DateTimeOfInspection,
            };
        }

        public static IEnumerable<InspectionHoursModel> FromDtos(IEnumerable<InspectionHoursDTO> dtos)
        {
            return dtos.Select(dto => new InspectionHoursModel
            {
                Id = dto.Id,
                ClientId = dto.ClientId,
                ClientFirstName = dto.ClientFirstName,
                ClientLastName = dto.ClientLastName,
                CarId = dto.CarId,
                CarBrandName = dto.CarBrandName,
                DateTimeOfInspection = dto.DateTimeOfInspection,
            });
        }

        public static InspectionHoursDTO ToDto(InspectionHoursModel model)
        {
            return new InspectionHoursDTO
            {
                Id = model.Id,
                ClientId = model.ClientId,
                ClientFirstName = model.ClientFirstName,
                ClientLastName = model.ClientLastName,
                CarId = model.CarId,
                CarBrandName = model.CarBrandName,
                DateTimeOfInspection = model.DateTimeOfInspection,
            };
        }
    }
}