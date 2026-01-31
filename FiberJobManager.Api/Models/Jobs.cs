using System.ComponentModel.DataAnnotations.Schema;

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

        public string HK { get; set; } //Hk
        public string SM { get; set; } // Sm
        public string NVT { get; set; }  // HK/NVT veya NVT
        public DateTime? FirstMeasurement { get; set; }  // İlk Ölçüm (FirstMeasurement)

        public string RevisionReason { get; set; }  // Admin'in revize nedeni
        public DateTime? RevisionDate { get; set; }  // Revizeye alınma tarihi

        //Revizyonu atayan kullanıcı
        public int? RevisionAssignedBy { get; set; }  // User ID
       
        [ForeignKey("RevisionAssignedBy")]  
        public User? RevisionAssignedByUser { get; set; }  // Navigation property

        //Revizyonun tamamlanması gereken tarih
        public DateTime? RevisionDueDate { get; set; }

        public int? RegionManager { get; set; }// Bölgenin Sahadaki Sorumlusu
        [ForeignKey("RegionManager")]
        public User? RegionManagerUser { get; set; }


        public int? CompanyId { get; set; }
        public int? RegionId { get; set; }

        // Navigation Properties
        [ForeignKey("CompanyId")]
        public Company? Company_Nav { get; set; }

        [ForeignKey("RegionId")]
        public Region? Region_Nav { get; set; }
    }
}
