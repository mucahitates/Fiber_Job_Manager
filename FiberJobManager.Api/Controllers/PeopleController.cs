using FiberJobManager.Api.Data;
using FiberJobManager.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace FiberJobManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PeopleController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet] //Tüm kişileri getirir
        public IActionResult GetPeople()
        {
            var people = _context.People.ToList();
            return Ok(people);
        }

        [HttpPost] // Yeni kişi ekle (Admin'e özel)
        public async Task<IActionResult> AddPerson([FromBody] Person model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.People.Add(model);
            await _context.SaveChangesAsync();

            return Ok(model);
        }
    }
}
