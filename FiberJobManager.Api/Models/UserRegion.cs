using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiberJobManager.Api.Models
{
    public class UserRegion
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public int RegionId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("RegionId")]
        public Region Region { get; set; }
    }
}