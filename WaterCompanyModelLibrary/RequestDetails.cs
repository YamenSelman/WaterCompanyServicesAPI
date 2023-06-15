using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ModelLibrary
{
    public class RequestDetails
    {
        [Key]
        public int Id { get; set; }

        public byte[]? Document { get; set; }

        public string? NewSubAddress { get; set; }
        public string? NewSubType { get; set; }

        public int RequestId { get; set; }
        public Request Request { get; set; }
    }
}
