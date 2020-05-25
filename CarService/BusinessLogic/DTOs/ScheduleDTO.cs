using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.DTOs
{
    public class ScheduleDTO : IBaseDTO
    {
        public int Id { get; set; }

        public DateTime ForDate { get; set; }

        public decimal HourBegin { get; set; }

        public decimal HourEnd { get; set; }

        public int EmployeeId { get; set; }
    }
}
