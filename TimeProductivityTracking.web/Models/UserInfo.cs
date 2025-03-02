using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Runtime.ExceptionServices;

namespace TimeProductivityTracking.web.Models
{
    public class UserInfo

    {
        [Key]
        [DisplayName("User ID")]
        public int UserId{ get; set; }
        [DisplayName("First Name")]
        public string FName { get; set; }
        [DisplayName("Last Name")]
        public string LName { get; set; }
        [DisplayName("Phone")]
        public string Phone { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Role")]
        public Roles Role { get; set; }

        [DisplayName("Hire Date")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }
        [Required(ErrorMessage = "Rate is required.")]
        [ForeignKey("Rate")]
        public int RateID { get; set; }
        public int Register { get; set; } = 0;
    }
    public enum Roles { Admin, Manager, HR, Member, User }

}
