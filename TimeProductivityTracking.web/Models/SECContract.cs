using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeProductivityTracking.web.Models
{
    public class SECContract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SECContractId { get; set; }
        public string SECName { get; set; }
        public string County { get; set; }
        public string Address { get; set; }
        public string PrimaryContract { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        
        public ICollection<Productivity> Productivities { get; set; }


    }
}
