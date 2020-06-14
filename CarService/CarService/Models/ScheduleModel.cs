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

        [DisplayName("Schedule Start Date")]
        public DateTime DateBegin { get; set; }

        [DisplayName("Schedule End Date")]
        public DateTime DateEnd { get; set; }

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
