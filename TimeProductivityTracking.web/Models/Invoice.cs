using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeProductivityTracking.web.Models
{
    public class Invoice
    {

            public int Id { get; set; }

            public string? InvoiceNumber { get; set; } 
            public DateTime InvoiceDate { get; set; }
            public string? Month { get; set; }

            public int ContractorId { get; set; }
            public UserInfo? Contractor { get; set; }
            [Precision(18, 2)]
            public decimal TotalHours { get; set; }
            [Precision(18, 2)]
            public decimal HourlyRate { get; set; }
            [Precision(18, 2)]
            public decimal TotalAmount { get; set; }
            public string? statusApproval { get; set; }






    }
}
