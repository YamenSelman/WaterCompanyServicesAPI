using ModelLibrary;

namespace WaterCompanyServiceWebSite.Models
{
    public class InvoicesVM
    {
        public Subscription Subscription { get; set; }
        public List<Invoice> Invoices { get; set;}
    }
}
