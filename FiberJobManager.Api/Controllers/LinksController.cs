using FiberJobManager.Api.Data;
using FiberJobManager.Api.Models;
using Microsoft.AspNetCore.Authorization;
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

        // 🔓 Herkes linkleri görebilir (Worker + Admin)
        [HttpGet]
        public IActionResult GetLinks()
        {
            var links = _context.TempDocuments
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new
                {
                    x.Id,
                    x.FileName,
                    x.Firma,
                    x.ProjeTuru,
                    Url = x.DriveUrl,
                    x.CreatedAt
                })
                .ToList();

            return Ok(links);
        }

        // 🔐 Sadece Admin link ekleyebilir
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddLink([FromBody] TempDocument model)
        {
            _context.TempDocuments.Add(model);
            await _context.SaveChangesAsync();
            return Ok(model);
        }
    }
}
