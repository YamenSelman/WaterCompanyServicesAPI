using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WaterCompanyServicesAPI
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

        [Required]
        [MaxLength(25), Display(Name = "Subscription Address")]
        public string SubscriptionAddress { get; set; }

        [AllowNull]
        public Consumer? Consumer { get; set; }

        [AllowNull]
        public virtual ICollection<Request>? Requests { get; set;}

        public override string ToString()
        {
            return $"Subscripion: {this.Id} - {this.ConsumerBarCode} - {this.ConsumerSubscriptionNo} - {this.SubscriptionAddress} - {this.SubscriptionUsingType} - {this.SubscriptionStatus}";
        }
    }
}
