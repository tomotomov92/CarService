using BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarService.Models
{
    public class EmployeeModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfStart { get; set; }

        public static EmployeeModel FromDto(EmployeeDTO dto)
        {
            return new EmployeeModel
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfStart = dto.DateOfStart,
            };
        }

        public static IEnumerable<EmployeeModel> FromDtos(IEnumerable<EmployeeDTO> dtos)
        {
            return dtos.Select(dto => new EmployeeModel
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfStart = dto.DateOfStart,
            });
        }

        public static EmployeeDTO ToDto(EmployeeModel model)
        {
            return new EmployeeDTO
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfStart = model.DateOfStart,
            };
        }
    }
}
