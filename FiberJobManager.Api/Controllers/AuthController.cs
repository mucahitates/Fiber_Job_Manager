using FiberJobManager.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiberJobManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: /api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.Users
                .FirstOrDefault(x => x.Email == request.Email && x.Password == request.Password);

            if (user == null)
                return Unauthorized("Email veya şifre hatalı");

            return Ok(new
            {
                Message = "Giriş başarılı",
                UserId = user.Id,
                userName=user.Name,
                Role = user.Role,
                Email=user.Email,
                Surname = user.UserSurname
            });
        }

    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
