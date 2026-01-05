using FiberJobManager.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FiberJobManager.Api.Models;   
using FiberJobManager.Api.Data;

namespace FiberJobManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TempDocumentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TempDocumentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/tempdocuments
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var docs = await _context.TempDocuments
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return Ok(docs);
        }

        // POST: api/tempdocuments
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TempDocument model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.CreatedAt = DateTime.UtcNow;

            _context.TempDocuments.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = model.Id }, model);
        }

    }
}
