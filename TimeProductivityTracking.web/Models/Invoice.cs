using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeProductivityTracking.web.Models
{
    public class Invoice
    {

            public int Id { get; set; }

            public string InvoiceNumber { get; set; }
            public DateTime InvoiceDate { get; set; }
            public string Month { get; set; }

            public int ContractorId { get; set; }
            public UserInfo Contractor { get; set; }

            public decimal TotalHours { get; set; }
            public decimal HourlyRate { get; set; }
            public decimal TotalAmount { get; set; }
      





    }
}
