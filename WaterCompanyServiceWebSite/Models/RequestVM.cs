using ModelLibrary;

namespace WaterCompanyServiceWebSite.Models
{
    public class RequestVM
    {
        public Request Request { get; set; }
        public DateTime? FinishDate { get; set; }
        public Department? RejectedBy { get; set; }
        public string? RejectNotes { get; set; }
    }
}
