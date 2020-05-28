using BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarService.Models
{
    public class ScheduleModel
    {
        public int Id { get; set; }

        public DateTime ForDate { get; set; }

        public decimal HourBegin { get; set; }

        public decimal HourEnd { get; set; }

        public int EmployeeId { get; set; }

        public static ScheduleModel FromDto(ScheduleDTO dto)
        {
            return new ScheduleModel
            {
                Id = dto.Id,
                ForDate = dto.ForDate,
                HourBegin = dto.HourBegin,
                HourEnd = dto.HourEnd,
                EmployeeId = dto.EmployeeId,
            };
        }

        public static IEnumerable<ScheduleModel> FromDtos(IEnumerable<ScheduleDTO> dtos)
        {
            return dtos.Select(dto => new ScheduleModel
            {
                Id = dto.Id,
                ForDate = dto.ForDate,
                HourBegin = dto.HourBegin,
                HourEnd = dto.HourEnd,
                EmployeeId = dto.EmployeeId,
            });
        }

        public static ScheduleDTO ToDto(ScheduleModel model)
        {
            return new ScheduleDTO
            {
                Id = model.Id,
                ForDate = model.ForDate,
                HourBegin = model.HourBegin,
                HourEnd = model.HourEnd,
                EmployeeId = model.EmployeeId,
            };
        }
    }
}
