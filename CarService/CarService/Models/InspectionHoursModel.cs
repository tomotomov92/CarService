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

        [DisplayName("Car Licenses Plate")]
        public string CarLicensePlate { get; set; }

        [DisplayName("Car Brand")]
        public string CarBrandName { get; set; }

        [DisplayName("Car Mileage")]
        public int Mileage { get; set; }

        [DisplayName("Inspection Date")]
        public DateTime DateTimeOfInspection { get; set; }

        public string Description { get; set; }

        public bool Archived { get; set; }

        public static InspectionHoursModel FromDto(InspectionHoursDTO dto)
        {
            return new InspectionHoursModel
            {
                Id = dto.Id,
                ClientId = dto.ClientId,
                ClientFirstName = dto.ClientFirstName,
                ClientLastName = dto.ClientLastName,
                CarId = dto.CarId,
                CarLicensePlate = dto.CarLicensePlate,
                CarBrandName = dto.CarBrandName,
                Mileage = dto.Mileage,
                DateTimeOfInspection = dto.DateTimeOfInspection,
                Description = dto.Description,
                Archived = dto.Archived,
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
                CarLicensePlate = dto.CarLicensePlate,
                CarBrandName = dto.CarBrandName,
                Mileage = dto.Mileage,
                DateTimeOfInspection = dto.DateTimeOfInspection,
                Description = dto.Description,
                Archived = dto.Archived,
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
                CarLicensePlate = model.CarLicensePlate,
                CarBrandName = model.CarBrandName,
                Mileage = model.Mileage,
                DateTimeOfInspection = model.DateTimeOfInspection,
                Description = model.Description,
                Archived = model.Archived,
            };
        }
    }
}