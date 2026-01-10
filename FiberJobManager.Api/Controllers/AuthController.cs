using FiberJobManager.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;     // JWT token üretmek için
using System.Security.Claims;             // Token içine koyacağımız userId, role vs için
using Microsoft.IdentityModel.Tokens;     // Token imzalama için
using System.Text;                        // Secret key’i byte[]’a çevirmek için

namespace FiberJobManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Veritabanı bağlantısı
        private readonly ApplicationDbContext _context;

        // appsettings.json içindeki Jwt ayarlarını okumak için
        private readonly IConfiguration _config;

        // DI ile hem DbContext hem de Configuration alıyoruz
        public AuthController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // POST: /api/auth/login
        // Kullanıcı giriş yapar, biz JWT üretip geri döneriz
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // 1️⃣ Email + şifre doğru mu diye DB’den kullanıcıyı buluyoruz
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == request.Email && x.Password == request.Password);

            // Kullanıcı yoksa direkt reddet
            if (user == null)
                return Unauthorized("Email veya şifre hatalı");

            // 2️⃣ Token içine koyacağımız bilgileri tanımlıyoruz
            // Bunlar daha sonra User.FindFirst(...) ile okunacak
            var claims = new[]
            {
                new Claim("userId", user.Id.ToString()),   // En kritik bilgi → kimin çağırdığını buradan bileceğiz
                new Claim(ClaimTypes.Name, user.Name),     // Kullanıcı adı
                new Claim(ClaimTypes.Role, user.Role)      // Yetki (Boss, User vs)
            };

            // 3️⃣ Token imzalamak için gizli anahtarımızı alıyoruz
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            // 4️⃣ Hangi algoritma ile imzalayacağımızı söylüyoruz
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 5️⃣ Gerçek JWT token burada üretiliyor
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],          // Kim üretti
                audience: _config["Jwt:Audience"],      // Kimler kullanabilir
                claims: claims,                          // İçine koyduğumuz userId, role vs
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_config["Jwt:ExpireMinutes"])
                ),                                       // Token ne zaman bitecek
                signingCredentials: creds               // İmza
            );

            // 6️⃣ Token’ı string’e çeviriyoruz
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            // 7️⃣ Client’a token + kullanıcı bilgilerini dönüyoruz
            // WPF bu token’ı saklayıp her API çağrısında gönderecek
            return Ok(new
            {
                token = jwt,
                userId = user.Id,
                userName = user.Name,
                role = user.Role,
                email = user.Email,
                surname = user.UserSurname
            });
        }
    }

    // Login ekranından gelen email + şifre modeli
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
