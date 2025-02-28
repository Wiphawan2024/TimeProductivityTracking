using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeProductivityTracking.web.Models
{
    public class Productivities
    {
        [Key]
        public int ProductivitiesId { get; set; }
        public DateOnly Date_Planned { get; set; }
        public DateTime SEC_Registered { get; set; }
        public int EMP_Application {  get; set; }
        public int EMP_Planned { get; set; }
        public int ActiveProject { get; set; }
        public DateTime YearToDate { get; set; }
        public int PlannedDays {  get; set; }
        public int Tasks_TBC_planned {  get; set; }
        public DateTime Date_Achieved {  get; set; }
        public int Achieved_days {  get; set; }
        public int Tasks_TBC_ach { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("SECContract")]
        public int SECContractId { get; set; }

       // public User User { get; set; }

      //  public SECContract SECContract { get; set; }

    }
}
