using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CarService.Models
{
    public class ScheduleModel
    {
        public int Id { get; set; }

        [Required, DisplayName("Schedule Start Date"), DataType(DataType.DateTime)]
        public DateTime DateBegin { get; set; }

        [Required, DisplayName("Schedule End Date"), DataType(DataType.DateTime)]
        public DateTime DateEnd { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public EmployeeModel Employee { get; set; }

        [Required, DisplayName("Schedule Date"), DataType(DataType.Date)]
        public DateTime ScheduleDate { get; set; } = DateTime.Now;

        [Required, DisplayName("Start Time"), DataType(DataType.Time)]
        public TimeSpan ScheduleStartTime { get; set; } = TimeSpan.Parse("08:30:00");

        [Required, DisplayName("End Time"), DataType(DataType.Time)]
        public TimeSpan ScheduleEndTime { get; set; } = TimeSpan.Parse("17:30:00");

        public SelectList EmployeeOptions { get; set; }

        public static ScheduleModel FromDto(ScheduleDTO dto)
        {
            return new ScheduleModel
            {
                Id = dto.Id,
                DateBegin = dto.DateBegin,
                DateEnd = dto.DateEnd,
                EmployeeId = dto.EmployeeId,
                Employee = EmployeeModel.FromDto(dto.Employee),
                ScheduleDate = dto.DateBegin.Date,
                ScheduleStartTime = dto.DateBegin.TimeOfDay,
                ScheduleEndTime = dto.DateEnd.TimeOfDay,
            };
        }

        public static IEnumerable<ScheduleModel> FromDtos(IEnumerable<ScheduleDTO> dtos)
        {
            return dtos.Select(dto => new ScheduleModel
            {
                Id = dto.Id,
                DateBegin = dto.DateBegin,
                DateEnd = dto.DateEnd,
                EmployeeId = dto.EmployeeId,
                Employee = EmployeeModel.FromDto(dto.Employee),
                ScheduleDate = dto.DateBegin.Date,
                ScheduleStartTime = dto.DateBegin.TimeOfDay,
                ScheduleEndTime = dto.DateEnd.TimeOfDay,
            });
        }

        public static ScheduleDTO ToDto(ScheduleModel model)
        {
            return new ScheduleDTO
            {
                Id = model.Id,
                DateBegin = model.DateBegin,
                DateEnd = model.DateEnd,
                EmployeeId = model.EmployeeId,
                Employee = EmployeeModel.ToDto(model.Employee),
            };
        }
    }
}
