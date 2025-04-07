using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeProductivityTracking.web.Models
{
    public class SECContract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SECContractId { get; set; }
        [DisplayName("SEC Name")]
        public string? SECName { get; set; }
        public string? County { get; set; }
        public string? Address { get; set; }
        [DisplayName("Primary Contact")]
        public string? PrimaryContract { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        
      //  public ICollection<Productivity> Productivities { get; set; }


    }
}
