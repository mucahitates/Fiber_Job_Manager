using FiberJobManager.Api.Data;
using FiberJobManager.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace FiberJobManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LinksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LinksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet] //Tüm linkleri getirir
        public IActionResult GetLinks()
        {
            var links = _context.Links.ToList();
            return Ok(links);
        }

        // Yeni link ekle (ADMIN'e özel)
        [HttpPost]
        public async Task<IActionResult> AddLink([FromBody] Link model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Links.Add(model);
            await _context.SaveChangesAsync();

            return Ok(model);
        }
    }
}
