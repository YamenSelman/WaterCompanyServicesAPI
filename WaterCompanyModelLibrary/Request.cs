using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WaterCompanyModelLibrary
{
    public class Request
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name ="Request Type")]
        public string RequestType { get; set; }

        [Required]
        [Display(Name = "Request Date")]
        public DateTime RequestDate { get; set; }

        [Required]
        public string RequestedRole { get; set; }

        public Consumer Consumer { get; set; }

        [Required]
        [Display(Name = "Request Status")]
        public string RequestStatus { get; set; }

        public Employee Employee { get; set; }
        public DateTime OutcomeDate { get; set; }
        public string Outcome { get; set; }

    }
}
