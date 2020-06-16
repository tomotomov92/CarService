using BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CarService.Models
{
    public class InspectionModel
    {
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }

        public ClientModel Client { get; set; }

        [Required]
        public int CarId { get; set; }

        public ClientCarModel ClientCar { get; set; }

        [Required, DisplayName("Inspection Car Mileage")]
        public int Mileage { get; set; }

        [Required, DisplayName("Inspection Date"), DataType(DataType.DateTime)]
        public DateTime DateTimeOfInspection { get; set; }

        public string Description { get; set; }

        public bool Archived { get; set; }

        [Required, DisplayName("Inspection Date"), DataType(DataType.Date)]
        public DateTime DateForInspection { get; set; } = DateTime.Now;

        [Required, DisplayName("Inspection Time"), DataType(DataType.Time)]
        public TimeSpan TimeForInspection { get; set; } = TimeSpan.Parse("09:00:00");

        public static InspectionModel FromDto(InspectionDTO dto)
        {
            return new InspectionModel
            {
                Id = dto.Id,
                ClientId = dto.ClientId,
                Client = ClientModel.FromDto(dto.Client),
                CarId = dto.CarId,
                ClientCar = ClientCarModel.FromDto(dto.ClientCar),
                Mileage = dto.Mileage,
                DateTimeOfInspection = dto.DateTimeOfInspection,
                Description = dto.Description,
                Archived = dto.Archived,
                DateForInspection = dto.DateTimeOfInspection.Date,
                TimeForInspection = dto.DateTimeOfInspection.TimeOfDay,
            };
        }

        public static IEnumerable<InspectionModel> FromDtos(IEnumerable<InspectionDTO> dtos)
        {
            return dtos.Select(dto => new InspectionModel
            {
                Id = dto.Id,
                ClientId = dto.ClientId,
                Client = ClientModel.FromDto(dto.Client),
                CarId = dto.CarId,
                ClientCar = ClientCarModel.FromDto(dto.ClientCar),
                Mileage = dto.Mileage,
                DateTimeOfInspection = dto.DateTimeOfInspection,
                Description = dto.Description,
                Archived = dto.Archived,
                DateForInspection = dto.DateTimeOfInspection.Date,
                TimeForInspection = dto.DateTimeOfInspection.TimeOfDay,
            });
        }

        public static InspectionDTO ToDto(InspectionModel model)
        {
            return new InspectionDTO
            {
                Id = model.Id,
                ClientId = model.ClientId,
                Client = ClientModel.ToDto(model.Client),
                CarId = model.CarId,
                ClientCar = ClientCarModel.ToDto(model.ClientCar),
                Mileage = model.Mileage,
                DateTimeOfInspection = model.DateTimeOfInspection,
                Description = model.Description,
                Archived = model.Archived,
            };
        }
    }
}