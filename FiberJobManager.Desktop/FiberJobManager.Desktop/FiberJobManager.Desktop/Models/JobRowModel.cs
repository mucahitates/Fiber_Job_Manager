using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiberJobManager.Desktop.Models
{
    public class JobRowModel
    {
        public int Id { get; set; }
        public string Firma { get; set; }
        public string Bolge { get; set; }
        public string HK { get; set; }
        public string SM { get; set; }
        public string HKNVT { get; set; }
        public string Erstmessung { get; set; }

        // 0 = yapılmadı, 1 = yapılamıyor, 2 = tamamlandı
        public int FieldStatus { get; set; }
        public string Note { get; set; }
    }
}
