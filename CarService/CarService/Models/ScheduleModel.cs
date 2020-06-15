using BusinessLogic.DTOs;
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

        [Required, DisplayName("Schedule Start Date"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime DateBegin { get; set; }

        [Required, DisplayName("Schedule End Date"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime DateEnd { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public EmployeeModel Employee { get; set; }

        public static ScheduleModel FromDto(ScheduleDTO dto)
        {
            return new ScheduleModel
            {
                Id = dto.Id,
                DateBegin = dto.DateBegin,
                DateEnd = dto.DateEnd,
                EmployeeId = dto.EmployeeId,
                Employee = EmployeeModel.FromDto(dto.Employee),
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
