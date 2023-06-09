using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ModelLibrary
{
    public class RequestDetails
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
