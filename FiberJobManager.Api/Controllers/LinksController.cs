using FiberJobManager.Api.Data;
using FiberJobManager.Api.Models;
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

        // GET: api/links
        // Tüm kullanıcılar linkleri görebilir
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetLinks()
        {
            var links = await _context.Links
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return Ok(links);
        }

        // POST: api/links
        // Sadece Admin (Boss) link ekleyebilir
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddLink([FromBody] Link link)
        {
            // Validasyon
            if (string.IsNullOrWhiteSpace(link.Title))
                return BadRequest("Başlık boş olamaz!");

            if (string.IsNullOrWhiteSpace(link.Url))
                return BadRequest("Link URL'si boş olamaz!");

            _context.Links.Add(link);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Link başarıyla eklendi",
                link = link
            });
        }

        // DELETE: api/links/{id}
        // Sadece Admin (Boss) link silebilir
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLink(int id)
        {
            var link = await _context.Links.FindAsync(id);

            if (link == null)
                return NotFound("Link bulunamadı");

            _context.Links.Remove(link);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Link başarıyla silindi" });
        }

        // PUT: api/links/{id}
        // Sadece Admin (Boss) link güncelleyebilir
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLink(int id, [FromBody] Link updatedLink)
        {
            var link = await _context.Links.FindAsync(id);

            if (link == null)
                return NotFound("Link bulunamadı");

            // Validasyon
            if (string.IsNullOrWhiteSpace(updatedLink.Title))
                return BadRequest("Başlık boş olamaz!");

            if (string.IsNullOrWhiteSpace(updatedLink.Url))
                return BadRequest("Link URL'si boş olamaz!");

            link.Title = updatedLink.Title;
            link.Url = updatedLink.Url;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Link başarıyla güncellendi",
                link = link
            });
        }
    }
}