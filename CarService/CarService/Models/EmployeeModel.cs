using BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CarService.Models
{
    public class EmployeeModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DisplayName("Employee Full Name")]
        public string FullName => $"{LastName}, {FirstName}";

        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }

        public string Password { get; set; }

        [DisplayName("Employee Hire Date")]
        public DateTime DateOfStart { get; set; }

        public int EmployeeRoleId { get; set; }

        [DisplayName("Employee Role")]
        public string EmployeeRoleName { get; set; }

        public bool Archived { get; set; }

        public static EmployeeModel FromDto(EmployeeDTO dto)
        {
            return new EmployeeModel
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
                Password = dto.Password,
                DateOfStart = dto.DateOfStart,
                EmployeeRoleId = dto.EmployeeRoleId,
                EmployeeRoleName = dto.EmployeeRoleName,
                Archived = dto.Archived,
            };
        }

        public static IEnumerable<EmployeeModel> FromDtos(IEnumerable<EmployeeDTO> dtos)
        {
            return dtos.Select(dto => new EmployeeModel
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
                Password = dto.Password,
                DateOfStart = dto.DateOfStart,
                EmployeeRoleId = dto.EmployeeRoleId,
                EmployeeRoleName = dto.EmployeeRoleName,
                Archived = dto.Archived,
            });
        }

        public static EmployeeDTO ToDto(EmployeeModel model)
        {
            return new EmployeeDTO
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,
                Password = model.Password,
                DateOfStart = model.DateOfStart,
                EmployeeRoleId = model.EmployeeRoleId,
                EmployeeRoleName = model.EmployeeRoleName,
                Archived = model.Archived,
            };
        }
    }
}
