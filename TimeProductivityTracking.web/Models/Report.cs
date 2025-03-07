namespace TimeProductivityTracking.web.Models
{
    public class Report
    {
        public int Id { get; set; }
        public List<Productivity> Productivities = new List<Productivity>();


    }
}
