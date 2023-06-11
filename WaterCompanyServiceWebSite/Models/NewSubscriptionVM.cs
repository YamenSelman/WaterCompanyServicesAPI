using System.ComponentModel.DataAnnotations;

namespace WaterCompanyServiceWebSite.Models
{
    public class NewSubscriptionVM
    {
        [Required]
        public string Address { get; set; }
        [Required]
        public string UsingType { get; set; }

        public IFormFile Document { get; set; }
    }
}
