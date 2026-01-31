using System.ComponentModel.DataAnnotations;

namespace FiberJobManager.Api.Models
{
    public class Region
    {

        [Key]
        public int RegionId { get; set; }

        [Required]
        [MaxLength(100)]
        public string RegionName { get; set; }

        public DateTime CreatedAt { get; set; }= DateTime.Now;

        //Navigasyon 
        public ICollection<Job> Jobs { get; set; }
        public ICollection<User> Users { get;set; }
    }
}
