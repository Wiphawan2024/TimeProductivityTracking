using TimeProductivityTracking.web.Models;

namespace TimeProductivityTracking.web.ViewModels
{
    public class InvoiceViewModel
    {
        public string? ContractorName {  get; set; }
        public string? ContractorEmail {  get; set; }
        public string? Month {  get; set; }
        public DateTime InvoiceDate { get; set; }

        public string? InvoiceNumber {  get; set; }
        public List<Productivity>? InvoiceProductivities { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal TotalHours {  get; set; }
        public decimal TotalAmount {  get; set; }   

    }
}
