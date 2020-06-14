using BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CarService.Models
{
    public class EmployeeRoleModel
    {
        public int Id { get; set; }

        [DisplayName("Employee Role Name")]
        public string EmployeeRoleName { get; set; }

        public bool Archived { get; set; }

        public static EmployeeRoleModel FromDto(EmployeeRoleDTO dto)
        {
            return new EmployeeRoleModel
            {
                Id = dto.Id,
                EmployeeRoleName = dto.EmployeeRoleName,
                Archived = dto.Archived,
            };
        }

        public static IEnumerable<EmployeeRoleModel> FromDtos(IEnumerable<EmployeeRoleDTO> dtos)
        {
            return dtos.Select(dto => new EmployeeRoleModel
            {
                Id = dto.Id,
                EmployeeRoleName = dto.EmployeeRoleName,
                Archived = dto.Archived,
            });
        }

        public static EmployeeRoleDTO ToDto(EmployeeRoleModel model)
        {
            return new EmployeeRoleDTO
            {
                Id = model.Id,
                EmployeeRoleName = model.EmployeeRoleName,
                Archived = model.Archived,
            };
        }
    }
}
