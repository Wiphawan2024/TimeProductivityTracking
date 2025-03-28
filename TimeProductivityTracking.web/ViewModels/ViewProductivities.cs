using System.ComponentModel.DataAnnotations.Schema;
using TimeProductivityTracking.web.Models;

namespace TimeProductivityTracking.web.ViewModels
{
    public class ViewProductivities
    {
        public int Id { get; set; }
        public int ProductivityId { get; set; } 

        [ForeignKey("Contractor")]
        public int? ContractorId { get; set; }
        public UserInfo Contractor { get; set; }

        public string FName { get; set; }
        public string LName { get; set; }
        public string FullName => $"{FName} {LName}";

      
        public string Month { get; set; }
        public decimal TotalDays { get; set; }

        // Filter input (optional)
        public string UserEmail { get; set; }
        public string SelectedMonth { get; set; }

        // Dropdown options
        public List<string> AvailableMonths { get; set; }
    }
}
