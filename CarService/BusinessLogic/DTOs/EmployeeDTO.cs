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

        public DateTime DateOfStart { get; set; }
    }
}
