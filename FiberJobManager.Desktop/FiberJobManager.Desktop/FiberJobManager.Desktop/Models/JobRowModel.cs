using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FiberJobManager.Desktop.Models
{
    public class JobRowModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        // Firma adı
        [JsonPropertyName("firma")]
        public string Firma { get; set; }

        // Bölge
        [JsonPropertyName("region")]
        public string Bolge { get; set; }
        
        [JsonPropertyName("hk")]
        public string HK { get; set; }

        [JsonPropertyName("sm")]
        public string SM { get; set; }

        [JsonPropertyName("nvt")]
        public string NVT { get; set; }

        // İlk ölçüm
        [JsonPropertyName("firstMeasurement")]
        public DateTime? IlkOlcum { get; set; }

        public string Status { get; set; }
    }
}