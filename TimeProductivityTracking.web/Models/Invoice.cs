using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeProductivityTracking.web.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Contractor")]
        public int ContractorId { get; set; }
        public UserInfo Contractor { get; set; }

        [Required]
        public DateTime Month { get; set; } // Represents the billing month

        [Required]
       // [Range(0, 8)] // Maximum 8 days per month per contractor
        public double TotalDaysWorked { get; set; }


        [Required]
        public double TotalPayment => TotalDaysWorked * 8 * (Contractor?.Rate?.HourlyWage ?? 0); // Auto-calculated

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
