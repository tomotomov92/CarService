using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.DTOs
{
    public class InspectionHoursDTO : IBaseDTO
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public string ClientFirstName { get; set; }

        public string ClientLastName { get; set; }

        public int CarId { get; set; }

        public string CarBrandName { get; set; }

        public string CarLicensePlate { get; set; }

        public DateTime DateTimeOfInspection { get; set; }
    }
}
