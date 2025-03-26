using System.ComponentModel.DataAnnotations;
namespace TimeProductivityTracking.API.Models
{
    public class UserInfo
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string FName { get; set; }

        [Required]
        public string LName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        public string Role { get; set; }

        public DateTime HireDate { get; set; }

        public int RateID { get; set; }

        public bool Register { get; set; } = false;
    }

}

