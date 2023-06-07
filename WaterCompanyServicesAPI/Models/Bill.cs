using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace WaterCompanyServicesAPI.Models
{
    public class Bill
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(4), Display(Name = "Bill Year")]
        public string Year { get; set; }

        [Required]
        [StringLength(2), Display(Name = "Bill No")]
        public string Number { get; set; }        
        
        [Required]
        [Display(Name = "Bill Amount")]
        public int Amount { get; set; }        
        [Required]
        [Display(Name = "Bill Status")]
        public bool Status { get; set; }

        public Subscription Subscription { get; set; }
    }
}
