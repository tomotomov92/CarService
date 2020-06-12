using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.DTOs
{
    public class EmployeeDTO : IBaseDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public DateTime DateOfStart { get; set; }

        public int EmployeeRoleId { get; set; }

        public string EmployeeRoleName { get; set; }

        public bool Archived { get; set; }
    }
}
