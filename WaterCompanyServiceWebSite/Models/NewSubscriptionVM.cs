using System.ComponentModel.DataAnnotations;

namespace WaterCompanyServiceWebSite.Models
{
    public class NewSubscriptionVM
    {
        public string Address { get; set; }
        public string UsingType { get; set; }
        public IFormFile Document { get; set; }
    }
}
