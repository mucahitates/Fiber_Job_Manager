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

        // Misafir kullanıcılar için firma bilgisi
        public string? Company { get; set; }  // Örn: "Quick City", "Berasco"
    }
}
