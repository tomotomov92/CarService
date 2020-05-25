using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.DTOs
{
    public class ClientCarDTO : IBaseDTO
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public int CarBrandId { get; set; }

        public int Mileage { get; set; }
    }
}
