using FiberJobManager.Api.Data;
using FiberJobManager.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace FiberJobManager.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/jobs  İş kayıtları
        [HttpGet]
        public async Task<IActionResult> GetJobs()
        {
            var jobs = await _context.Jobs.ToListAsync();
            return Ok(jobs);
        }

        // GET: api/jobs/5  Tek bir işin kaydı
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null) return NotFound();
            return Ok(job);
        }

        // POST: api/jobs Yeni iş Açar
        [HttpPost]
        public async Task<IActionResult> CreateJob([FromBody] Job job)
        {
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
            return Ok(job);
        }

        // PUT: api/jobs/5 İş kaydını günceller (İzin gerektirmeyen Endpoint)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(int id, [FromBody] Job updated)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null) return NotFound();

            job.Title = updated.Title;
            job.Description = updated.Description;
            job.Status = updated.Status;
            job.AssignedUserId = updated.AssignedUserId;

            await _context.SaveChangesAsync();
            return Ok(job);
        }

        // DELETE: api/jobs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null) return NotFound();

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //Job=User eşleştirme
        [HttpPost("{jobId}/assign/{userId}")]
        public async Task<IActionResult> AssignJob(int jobId, int userId)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null)
                return NotFound("Job not found");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found");

            job.AssignedUserId = userId;
            await _context.SaveChangesAsync();

            return Ok(job);
        }

        //Kullanıcı kendi işini güncelleyebilir (İzin Kontrollü Endpoint)
        [HttpPut("{jobId}/update/{userId}")]
        public async Task<IActionResult> UpdateJobWithPermission(int jobId, int userId, [FromBody] Job updated)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null)
                return NotFound("Job not found");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found");

            if (user.Role != "Admin" && job.AssignedUserId != userId)
                return Forbid("Bu iş üzerinde işlem yapamazsın.");

            job.Title = updated.Title;
            job.Description = updated.Description;
            job.Status = updated.Status;
            job.AssignedUserId = updated.AssignedUserId;

            // 🔹 AUTOMATIC REVISION
            _context.Revisions.Add(new Revision
            {
                JobId = jobId,
                UserId = userId,
                Note = $"Job updated. Status: {updated.Status}, AssignedUserId: {updated.AssignedUserId}"
            });

            await _context.SaveChangesAsync();

            await _context.SaveChangesAsync();

            return Ok(job);
        }

        // İstenen işe ait tüm revizeleri listeler
        [HttpGet("{jobId}/revisions")]
        public async Task<IActionResult> GetJobRevisions(int jobId)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null)
                return NotFound("Job not found");

            var revisions = await _context.Revisions
                .Where(r => r.JobId == jobId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Ok(revisions);
        }


        [HttpGet("my-new")]
        public async Task<IActionResult> GetMyNewJobs()
        {
            // Token'dan userId al
            var userId = int.Parse(User.FindFirst("userId").Value);

            // Kullanıcıya atanmış işleri çek
            var jobs = await _context.Jobs
                .Where(j => j.AssignedUserId == userId)
                .Select(j => new
                {
                    j.Id,
                    j.Title,
                    j.Description,
                    j.Firma,
                    j.Region,
                    j.HK,
                    j.SM,
                    j.NVT,
                    j.FirstMeasurement,
                    j.Status,
                    // 🔥 YENİ: En son field report'tan FieldStatus'u al
                    FieldStatus = _context.JobFieldReports
                        .Where(r => r.JobId == j.Id)
                        .OrderByDescending(r => r.CreatedAt)
                        .Select(r => r.FieldStatus)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(jobs);
        }



    }
}
