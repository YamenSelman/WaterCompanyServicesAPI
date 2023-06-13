using ModelLibrary;
using System.ComponentModel.DataAnnotations;

namespace WaterCompanyServiceWebSite.Models
{
    public class TransferRequestVM
    {

        public Subscription Subscription { get; set; }

        public IFormFile Document { get; set; }
    }
}
