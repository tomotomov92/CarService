using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.DTOs
{
    public class EmployeeRoleDTO : IBaseDTO
    {
        public int Id { get; set; }

        public string EmployeeRoleName { get; set; }

        public bool Archived { get; set; }
    }
}
