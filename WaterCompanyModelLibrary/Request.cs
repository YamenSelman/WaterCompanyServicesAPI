using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ModelLibrary
{
    public class Request
    {
        [Key]
        public int Id { get; set; }
        public string RequestType { get; set; }

        [Required]
        [Display(Name = "Request Date")]
        public DateTime RequestDate { get; set; }

        [AllowNull]
        public Consumer? Consumer { get; set; }
        [AllowNull]
        public Subscription? Subscription { get; set; }

        [Required]
        [Display(Name = "Request Status")]
        public string RequestStatus { get; set; }
        public Department? CurrentDepartment { get; set; }

        public RequestDetails? Details { get; set; }
        public RequestResult? Result { get; set; }

    }
}
