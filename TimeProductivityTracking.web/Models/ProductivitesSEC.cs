using TimeProductivityTracking.web.Models;
namespace TimeProductivityTracking.web.Models
{
    public class ProductivitesSEC
    {
        public int Id { get; set; }
      //  public int SecId { get; set; }
       // public int ProductId { get; set; }

        public List<Productivities> reports =new List<Productivities>();
        public List<SECContract> contracts =new List<SECContract>();

    }
}
