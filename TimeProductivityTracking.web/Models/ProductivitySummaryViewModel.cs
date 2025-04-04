using System.ComponentModel.DataAnnotations.Schema;

namespace TimeProductivityTracking.web.Models
{
    public class ProductivitySummaryViewModel
    {
        public int Id { get; set; }
        [ForeignKey("Contractor")]
        public int ContractorId { get; set; }

        public UserInfo? Contractor { get; set; }

        public string? FName { get; set; }
        public string? LName { get; set; }
        public string FullName => $"{FName} {LName}";

        public string? SecName { get; set; }
        public string? Month { get; set; }
        public decimal TotalAchevedDays { get; set; }

       
        public string? UserEmail { get; set; }
        public string? SelectedMonth { get; set; }

        // Dropdown options
        public List<string>? AvailableMonths { get; set; }

    }
}
