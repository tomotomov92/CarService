using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CarService.Models
{
    public class EmployeeModel : CredentialModel
    {
        [DisplayName("Employee Full Name")]
        public string FullName => $"{LastName}, {FirstName}";

        public EmployeeRoleModel EmployeeRole { get; set; }

        public SelectList EmployeeRoleOptions { get; set; }

        public static EmployeeModel FromDto(EmployeeDTO dto)
        {
            return new EmployeeModel
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
                Password = dto.Password,
                RepeatPassword = dto.RepeatPassword,
                DateOfStart = dto.DateOfStart,
                EmployeeRoleId = dto.EmployeeRoleId,
                EmployeeRole = EmployeeRoleModel.FromDto(dto.EmployeeRole),
                RequirePasswordChange = dto.RequirePasswordChange,
                Archived = dto.Archived,
                ErrorMessage = dto.ErrorMessage,
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
                RepeatPassword = dto.RepeatPassword,
                DateOfStart = dto.DateOfStart,
                EmployeeRoleId = dto.EmployeeRoleId,
                EmployeeRole = EmployeeRoleModel.FromDto(dto.EmployeeRole),
                RequirePasswordChange = dto.RequirePasswordChange,
                Archived = dto.Archived,
                ErrorMessage = dto.ErrorMessage,
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
                RepeatPassword = model.RepeatPassword,
                DateOfStart = model.DateOfStart,
                EmployeeRoleId = model.EmployeeRoleId,
                EmployeeRole = EmployeeRoleModel.ToDto(model.EmployeeRole),
                RequirePasswordChange = model.RequirePasswordChange,
                Archived = model.Archived,
                ErrorMessage = model.ErrorMessage,
            };
        }
    }
}
