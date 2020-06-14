using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.DTOs
{
    public class InspectionDTO : IBaseDTO
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public ClientDTO Client { get; set; }

        public int CarId { get; set; }

        public ClientCarDTO ClientCar { get; set; }

        public int Mileage { get; set; }

        public DateTime DateTimeOfInspection { get; set; }

        public string Description { get; set; }

        public bool Archived { get; set; }
    }
}
