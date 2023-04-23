using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WaterCompanyModelLibrary
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name ="Invoice Year")]
        [Range(2000,2050)]
        public int InvoiceYear { get; set; }

        [Required]
        [Display(Name = "Invoice cycle"),Range(1,6)]
        public int InvoiceCycle { get; set; }

        [Required]
        [Display(Name = "Invoice Value"),DefaultValue(0)]
        public int InvoiceValue { get; set; }

        [Required]
        [Display(Name = "Invoice Status")]
        public int InvoiceStatus { get; set; }
        
        public Subscription Subscription { get; set; }
    }
}
