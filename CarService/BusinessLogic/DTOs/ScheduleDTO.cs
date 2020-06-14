using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.DTOs
{
    public class ScheduleDTO : IBaseDTO
    {
        public int Id { get; set; }

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }

        public int EmployeeId { get; set; }

        public EmployeeDTO Employee { get; set; }
    }
}
