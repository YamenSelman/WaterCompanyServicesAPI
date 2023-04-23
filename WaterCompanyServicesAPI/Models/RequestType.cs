using System.ComponentModel.DataAnnotations;

namespace WaterCompanyServicesAPI.Models
{
    public class RequestType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Request Type")]
        public string TypeName { get; set; }
        public virtual ICollection<FlowStep> Steps { get; set; }

    }
}
