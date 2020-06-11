using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.DTOs
{
    public class ClientCarDTO : IBaseDTO
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public string ClientFirstName { get; set; }

        public string ClientLastName { get; set; }

        public int CarBrandId { get; set; }

        public string CarBrandName { get; set; }

        public int Mileage { get; set; }
    }
}
