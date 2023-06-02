using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WaterCompanyServicesAPI.Models
{
    public class RequestDocument
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(25), Display(Name = "Document Type")]
        public string DocumentType { get; set; }

        [Required]
        public byte[] DocumentPath { get; set; }

        public Request Request { get; set; }
    }
}
