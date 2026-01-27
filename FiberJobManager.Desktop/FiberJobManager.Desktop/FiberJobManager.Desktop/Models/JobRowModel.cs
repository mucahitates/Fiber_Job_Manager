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
        public DateTime? FirstMeasurement { get; set; }

        // İşin genel durumu (API'den gelen)
        public string Status { get; set; }

        // 🔥 YENİ: Saha durumu (ComboBox için)
        // 0 = Yapılmadı, 1 = Yapılamıyor, 2 = Tamamlandı
        public int FieldStatus { get; set; } = 0;

        [JsonPropertyName("completedDate")]
        public DateTime? CompletedDate { get; set; }

        
        //Revize 

        [JsonPropertyName("revisionReason")]
        public string RevisionReason { get; set; }

        [JsonPropertyName("revisionDate")]
        public DateTime? RevisionDate { get; set; }

        // Revizyonu atayan kullanıcı adı
        [JsonPropertyName("revisionAssignedByName")]
        public string RevisionAssignedByName { get; set; }
    }
}