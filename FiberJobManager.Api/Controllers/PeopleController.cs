using FiberJobManager.Api.Data;
using FiberJobManager.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiberJobManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // JWT zorunlu
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PeopleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Tüm kişileri getir
        [HttpGet]
        public async Task<IActionResult> GetPeople()
        {
            var people = await _context.People
                .OrderBy(x => x.Company)
                .ThenBy(x => x.Name)
                .ToListAsync();

            return Ok(people);
        }

        // Admin yeni kişi ekleyebilsin
        [HttpPost]
        [Authorize(Roles = "Boss")]
        public async Task<IActionResult> AddPerson([FromBody] Person person)
        {
            _context.People.Add(person);
            await _context.SaveChangesAsync();
            return Ok(person);
        }
    }
}
