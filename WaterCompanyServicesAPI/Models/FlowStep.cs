using System.ComponentModel.DataAnnotations;

namespace WaterCompanyServicesAPI.Models
{
    public class FlowStep
    {
        [Key]
        public int Id { get; set; }
        public Department Department { get; set; }
        public RequestType RequestType { get; set; }
        public int Order { get; set; }
    }
}
