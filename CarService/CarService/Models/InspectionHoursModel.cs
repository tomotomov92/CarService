using BusinessLogic.DTOs;
using System;

namespace CarService.Models
{
    public class InspectionHoursModel
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public int CarId { get; set; }

        public DateTime DateTimeOfInspection { get; set; }

        public static InspectionHoursModel FromDto(InspectionHoursDTO dto)
        {
            return new InspectionHoursModel
            {
                Id = dto.Id,
                ClientId = dto.ClientId,
                CarId = dto.CarId,
                DateTimeOfInspection = dto.DateTimeOfInspection,
            };
        }

        public static InspectionHoursDTO ToDto(InspectionHoursModel model)
        {
            return new InspectionHoursDTO
            {
                Id = model.Id,
                ClientId = model.ClientId,
                CarId = model.CarId,
                DateTimeOfInspection = model.DateTimeOfInspection,
            };
        }
    }
}