using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.DTOs
{
    public class InspectionHoursDTO : IBaseDTO
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public int CarId { get; set; }

        public DateTime DateTimeOfInspection { get; set; }
    }
}
