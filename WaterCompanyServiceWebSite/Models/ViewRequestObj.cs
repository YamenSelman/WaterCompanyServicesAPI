using ModelLibrary;
using System.Drawing;
using System.IO;

namespace WaterCompanyServiceWebSite.Models
{
    public class ViewRequestObj
    {
        public Request Request { get; set; }
        public RequestsLog Log { get; set; }
        public IFormFile UploadedFile { get; set; }
    }
}
