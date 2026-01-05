namespace FiberJobManager.Api.Models
{
    public class TempDocument
    {
        public int Id { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string Firma { get; set; } = string.Empty;

        public string ProjeTuru { get; set; } = string.Empty;

        public string DriveUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
