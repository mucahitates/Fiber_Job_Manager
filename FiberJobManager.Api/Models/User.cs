using System.ComponentModel.DataAnnotations.Schema;

namespace FiberJobManager.Api.Models
{
    public class User
    {
        public int Id { get; set; }          // Primary Key
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "Worker";   // Admin / Worker
        public string Password { get; set; }
        public string UserSurname { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? CompanyId { get; set; }
        public int? RegionId { get; set; }

        [ForeignKey("CompanyId")]
        public Company? Company_Nav { get; set; }

        [ForeignKey("RegionId")]
        public Region? Region_Nav { get; set; }
    }
}
