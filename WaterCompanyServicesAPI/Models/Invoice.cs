using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WaterCompanyServicesAPI
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name ="Invoice Year")]
        public int InvoiceYear { get; set; }

        [Required]
        [Display(Name = "Invoice cycle"),Range(1,6)]
        public int InvoiceCycle { get; set; }

        [Required]
        [Display(Name = "Invoice Value"),DefaultValue(0)]
        public int InvoiceValue { get; set; }

        [Required]
        [Display(Name = "Invoice Status")]
        public bool InvoiceStatus { get; set; }

        public Subscription Subscription { get; set; }
    }
}
