using System.ComponentModel.DataAnnotations;

namespace TimeProductivityTracking.web.Models
{
    public class Contractor:SECContract
    {
     
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Monthly")]

        public DateTime Monthly { get; set; }
        public int UserId_FK { get; set; }
        public UserInfo UserInfo { get; set; }
        public ICollection<Productivities> Productivities { get; set; }

    }
}
