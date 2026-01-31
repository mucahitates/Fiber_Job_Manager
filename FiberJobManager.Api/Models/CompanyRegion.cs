using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiberJobManager.Api.Models
{
    public class CompanyRegion
    {
        [Key]
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public int RegionId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }

        [ForeignKey("RegionId")]
        public Region Region { get; set; }
    }
}