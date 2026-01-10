using System.Text.Json.Serialization;


namespace FiberJobManager.Desktop.Models
{
    public class LoginResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }   // 🔐 JWT buraya geliyor

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("surname")]
        public string Surname { get; set; }


    }
}
