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

            // 🔥 YENİ: Hem Completed hem Revision'ı hariç tut
            var jobs = await _context.Jobs
                .Where(j => j.AssignedUserId == userId
                         && j.Status != "Completed"
                         && j.Status != "Revision")  // ← Bu satırı ekle
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
                    // En son field report'tan FieldStatus'u al
                    FieldStatus = _context.JobFieldReports
                        .Where(r => r.JobId == j.Id)
                        .OrderByDescending(r => r.CreatedAt)
                        .Select(r => r.FieldStatus)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(jobs);
        }

        // GET: api/jobs/completed
        // Kullanıcıya ait tamamlanmış işleri listeler
        [HttpGet("completed")]
        public async Task<IActionResult> GetCompletedJobs()
        {
            // 🔥 Token'dan userId al
            var userId = int.Parse(User.FindFirst("userId").Value);

            // 🔥 Sadece bu kullanıcıya atanmış VE Completed olan işleri çek
            var jobs = await _context.Jobs
                .Where(j => j.AssignedUserId == userId && j.Status == "Completed")
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
                    FieldStatus = _context.JobFieldReports
                        .Where(r => r.JobId == j.Id)
                        .OrderByDescending(r => r.CreatedAt)
                        .Select(r => r.FieldStatus)
                        .FirstOrDefault(),
                    // Tamamlanma tarihi (FieldStatus = 2 olan en son rapor)
                    CompletedDate = _context.JobFieldReports
                        .Where(r => r.JobId == j.Id && r.FieldStatus == 2)
                        .OrderByDescending(r => r.CreatedAt)
                        .Select(r => r.CreatedAt)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(jobs);
        }
        // GET: api/jobs/my-revision
        // Kullanıcıya atanmış revize bekleyen işleri listeler
        [HttpGet("my-revision")]
        public async Task<IActionResult> GetMyRevisionJobs()
        {
            // Token'dan userId al
            var userId = int.Parse(User.FindFirst("userId").Value);

            // Status = "Revision" olan işleri çek
            var jobs = await _context.Jobs
                .Where(j => j.AssignedUserId == userId && j.Status == "Revision")
                .Include(j => j.RevisionAssignedByUser)  // 🔥 Revizeyi atayan kullanıcıyı dahil et
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
                    j.RevisionReason,
                    j.RevisionDate,
                    RevisionAssignedByName = j.RevisionAssignedByUser != null
                        ? j.RevisionAssignedByUser.Name
                        : "Bilinmiyor",  // 🔥 Revizeyi atayan kişinin adı
                    FieldStatus = _context.JobFieldReports
                        .Where(r => r.JobId == j.Id)
                        .OrderByDescending(r => r.CreatedAt)
                        .Select(r => r.FieldStatus)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(jobs);
        }


        // GET: api/jobs/{jobId}/revision-note
        // İşin revize nedeni ve worker notunu getirir
        [HttpGet("{jobId}/revision-note")]
        public async Task<IActionResult> GetRevisionNote(int jobId)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null)
                return NotFound("İş kaydı bulunamadı");

            // En son field report'u çek (worker notu için)
            var latestReport = await _context.JobFieldReports
                .Where(r => r.JobId == jobId)
                .OrderByDescending(r => r.CreatedAt)
                .FirstOrDefaultAsync();

            var response = new
            {
                revisionReason = job.RevisionReason ?? "Revize nedeni girilmemiş.",
                workerNote = latestReport?.Note ?? ""
            };

            return Ok(response);
        }

        
        // PUT: api/jobs/{jobId}/set-revision
        // Admin veya misafir kullanıcı bir işi revizeye alır
        [Authorize]
        [HttpPut("{jobId}/set-revision")]
        public async Task<IActionResult> SetJobRevision(int jobId, [FromBody] SetRevision dto)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null)
                return NotFound("İş kaydı bulunamadı");

            // Giriş yapan kullanıcı
            var userId = int.Parse(User.FindFirst("userId").Value);
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return Unauthorized("Kullanıcı bulunamadı");

            // 🔥 YENİ: Misafir kullanıcı kontrolü
            // Misafir kullanıcı sadece kendi firmasındaki işleri revizeye alabilir
            if (user.Role == "Guest" && job.Firma != user.Company)
            {
                return Forbid("Sadece kendi firmanıza ait işleri revizeye alabilirsiniz!");
            }

            // Admin ve Boss her şeyi yapabilir
            if (user.Role != "Admin" && user.Role != "Boss" && user.Role != "Guest")
            {
                return Forbid("Revize atama yetkiniz yok!");
            }

            // Türkiye saati
            var turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            var turkeyTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, turkeyTimeZone);

            job.Status = "Revision";
            job.RevisionReason = dto.RevisionReason;
            job.RevisionDate = turkeyTime;
            job.RevisionAssignedBy = userId;  // 🔥 Kim revizeye aldı

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "İş revizeye alındı",
                job = job,
                assignedBy = user.Name
            });
        }

        // GET: api/jobs/counts
        // Kullanıcıya ait iş sayaçlarını döndürür
        [HttpGet("counts")]
        public async Task<IActionResult> GetJobCounts()
        {
            var userId = int.Parse(User.FindFirst("userId").Value);

            var newJobsCount = await _context.Jobs
                .Where(j => j.AssignedUserId == userId && j.Status != "Completed" && j.Status != "Revision")
                .CountAsync();

            var revisionJobsCount = await _context.Jobs
                .Where(j => j.AssignedUserId == userId && j.Status == "Revision")
                .CountAsync();

            var completedJobsCount = await _context.Jobs
                .Where(j => j.AssignedUserId == userId && j.Status == "Completed")
                .CountAsync();

            return Ok(new
            {
                newJobs = newJobsCount,
                revisionJobs = revisionJobsCount,
                completedJobs = completedJobsCount
            });
        }


    }
}
