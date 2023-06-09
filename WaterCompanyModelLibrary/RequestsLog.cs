using System.ComponentModel.DataAnnotations;

namespace ModelLibrary
{
    public class RequestsLog
    {
        [Key]
        public int Id { get; set; }
        public Request Request { get; set; }
        public DateTime DateTime { get; set; }
        public Department Department { get; set; }
        public Employee Employee { get; set; }
        public string Notes { get; set; }
        public Boolean Decision { get; set; }
    }
}
