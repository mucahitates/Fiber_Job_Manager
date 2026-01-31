using System.ComponentModel.DataAnnotations;

namespace FiberJobManager.Api.Models
{
    
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CompanyName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public ICollection<Job> Jobs { get; set; }
        public ICollection<User> Users { get; set; }
    }

}
