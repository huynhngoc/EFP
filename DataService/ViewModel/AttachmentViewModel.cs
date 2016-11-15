using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{
    public class AttachmentViewModel
    {
        public string Type { get; set; } //img, other
        public string Url { get; set; }
        public string Filename { get; set; }
    }
}
