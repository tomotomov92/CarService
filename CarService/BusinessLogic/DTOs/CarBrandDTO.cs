using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.DTOs
{
    public class CarBrandDTO : IBaseDTO
    {
        public int Id { get; set; }

        public string BrandName { get; set; }
    }
}
