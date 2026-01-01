using FiberJobManager.Api.Data;
using FiberJobManager.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiberJobManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RevisionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RevisionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/revisions  Revizyon notu ekler
        [HttpPost]
        public async Task<IActionResult> AddRevision([FromBody] Revision revision)
        {
            var job = await _context.Jobs.FindAsync(revision.JobId);
            if (job == null)
                return NotFound("Job not found");

            var user = await _context.Users.FindAsync(revision.UserId);
            if (user == null)
                return NotFound("User not found");

            _context.Revisions.Add(revision);
            await _context.SaveChangesAsync();

            return Ok(revision);
        }
    }
}
