namespace FiberJobManager.Api.Models
{
    public class Job
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public int? AssignedUserId { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        
        public string Firma { get; set; }   // firma
        public string Region { get; set; }       // Bölge

        public string HK { get; set; }
        public string SM { get; set; }
        public string NVT { get; set; }          // HK/NVT veya NVT

        public DateTime? FirstMeasurement { get; set; }  // İlk Ölçüm (Erstmessung)

    }
}
