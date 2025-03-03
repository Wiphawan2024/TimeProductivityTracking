using System.ComponentModel.DataAnnotations;

namespace OfficeManagement.Models
{
    public class Rate
    {
        [Key]
        public int RateID { get; set; }
        public string RateName { get; set; }
        public double HourlyWage { get; set; }

    }
}
