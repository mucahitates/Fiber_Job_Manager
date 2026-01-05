using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiberJobManager.Desktop.Models
{
    public class TempDocument
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Firma { get; set; }
        public string ProjeTuru { get; set; }
        public string DriveUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
