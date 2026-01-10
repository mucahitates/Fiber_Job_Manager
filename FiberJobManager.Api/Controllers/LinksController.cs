using FiberJobManager.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // ✅ Herkes görebilsin (JWT varsa çalışır, [Authorize] koymadım)
        // Eğer genel olarak global authorize yaptıysan ve bunun da JWT istemesini istiyorsan,
        // [Authorize] ekleyebilirsin.
        [HttpGet]
        public async Task<IActionResult> GetLinks()
        {
            // tempdocuments tablosundan "link gibi" döndürüyoruz
            var links = await _context.TempDocuments
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new
                {
                    id = x.Id,
                    title = (x.Firma ?? "") + " - " + (x.FileName ?? ""),
                    url = x.DriveUrl
                })
                .ToListAsync();

            return Ok(links);
        }
    }
}
