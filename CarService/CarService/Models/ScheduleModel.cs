using BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CarService.Models
{
    public class ScheduleModel
    {
        public int Id { get; set; }

        [DisplayName("Schedule Date")]
        public DateTime ForDate { get; set; }

        [DisplayName("Schedule Start Time")]
        public decimal HourBegin { get; set; }

        [DisplayName("Schedule End Time")]
        public decimal HourEnd { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeFirstName { get; set; }

        public string EmployeeLastName { get; set; }

        [DisplayName("Employee Full Name")]
        public string EmployeeFullName => $"{EmployeeLastName}, {EmployeeFirstName}";

        public static ScheduleModel FromDto(ScheduleDTO dto)
        {
            return new ScheduleModel
            {
                Id = dto.Id,
                ForDate = dto.ForDate,
                HourBegin = dto.HourBegin,
                HourEnd = dto.HourEnd,
                EmployeeId = dto.EmployeeId,
                EmployeeFirstName = dto.EmployeeFirstName,
                EmployeeLastName = dto.EmployeeLastName,
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
                EmployeeFirstName = dto.EmployeeFirstName,
                EmployeeLastName = dto.EmployeeLastName,
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
                EmployeeFirstName = model.EmployeeFirstName,
                EmployeeLastName = model.EmployeeLastName,
            };
        }
    }
}
