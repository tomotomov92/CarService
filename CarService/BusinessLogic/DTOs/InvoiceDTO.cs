using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.DTOs
{
    public class InvoiceDTO : IBaseDTO
    {
        public int Id { get; set; }

        public int InspectionId { get; set; }

        public InspectionDTO Inspection { get; set; }

        public DateTime InvoiceDate { get; set; }

        public decimal InvoiceSum { get; set; }

        public string Description { get; set; }

        public bool Archived { get; set; }
    }
}
