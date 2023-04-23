using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WaterCompanyModelLibrary
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(6), Display(Name = "Consumer BarCode")]
        public string ConsumerBarCode { get; set; }

        [Required]
        [StringLength(6), Display(Name = "Consumer Subscription No")]
        public string ConsumerSubscriptionNo { get; set; }

        [Required]
        [Display(Name = "Subsrciption Type")]
        public string SubscriptionUsingType { get; set; }

        [Required]
        [Display(Name = "Subscription status")]
        public string SubscriptionStatus { get; set; }

        [AllowNull]
        public Consumer Consumer { get; set; }

        [AllowNull]
        public ICollection<Request> Requests { get; set;}
    }
}
