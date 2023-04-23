using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using WaterCompanyServicesAPI.Models;

namespace WaterCompanyServicesAPI
{
    public class Request
    {
        [Key]
        public int Id { get; set; }
        public RequestType RequestType { get; set; }

        [Required]
        [Display(Name = "Request Date")]
        public DateTime RequestDate { get; set; }

        [AllowNull]
        public Consumer Consumer { get; set; }
        [AllowNull]
        public Subscription Subscription { get; set; }

        [Required]
        [Display(Name = "Request Status")]
        public string RequestStatus { get; set; }
        public Employee Employee { get; set; }

        public virtual ICollection<RequestDocument> Documents { get; set; }

    }
}
