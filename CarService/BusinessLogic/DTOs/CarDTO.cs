using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.DTOs
{
    public class CarDTO : IBaseDTO
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public ClientDTO Client { get; set; }

        public int CarBrandId { get; set; }

        public CarBrandDTO CarBrand { get; set; }

        public string LicensePlate { get; set; }

        public int Mileage { get; set; }

        public bool Archived { get; set; }
    }
}
