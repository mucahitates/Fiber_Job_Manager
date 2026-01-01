namespace FiberJobManager.Api.Models
{
    public class Revision
    {
        public int Id { get; set; }

        public int JobId { get; set; }

        public int UserId { get; set; }

        public string Note { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
