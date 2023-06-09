using ModelLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ModelLibrary
{
    public class RequestResult
    {
        [Key]
        public int Id { get; set; }
        public byte[] Document { get; set; }
        public int RequestId { get; set; }
        public Request Request { get; set; }
    }
}
