using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TimeProductivityTracking.web.Models
{
    public class Rate
    {
        [Key]
        public int RateID { get; set; }
        [DisplayName("Rate Name")]
        public string? RateName { get; set; }
        [DisplayName("Hourly Rate")]
        public double HourlyWage { get; set; }

    }
}
