using Microsoft.AspNetCore.Mvc;           
using Microsoft.EntityFrameworkCore;     
using FiberJobManager.Api.Data;
using FiberJobManager.Api.Models;        

namespace FiberJobManager.Api.Controllers
{
    
    [ApiController]

    // Bu controller'ın base URL'si: /api/jobs
    [Route("api/jobs")]
    public class JobFieldReportsController : ControllerBase
    {
        // Veritabanı bağlantısı
        private readonly ApplicationDbContext _context;

        // Dependency Injection ile DbContext alınır
        public JobFieldReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        // POST /api/jobs/{jobId}/field-report
        // Bir proje için yeni saha raporu kaydeder
        [HttpPost("{jobId}/field-report")]
        public async Task<IActionResult> CreateReport(int jobId, [FromBody] FieldReportDto dto)
        {
            // Türkiye saati hesapla
            var turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            var turkeyTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, turkeyTimeZone);

            // 1️⃣ İş kaydı var mı kontrol et
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null)
                return NotFound("İş kaydı bulunamadı");

            // 2️⃣ Giriş yapmış kullanıcının ID'sini al
            var userId = int.Parse(User.FindFirst("userId").Value);

            // 🔥 YENİ: Tamamlandı seçiliyse NOT zorunlu
            // 4️⃣ Status güncellemesi
            if (dto.Status == 1 && !string.IsNullOrWhiteSpace(dto.Note))
            {
                // Yapılamıyor → Revizeye al
                job.Status = "Revision";
                job.RevisionReason = dto.Note;
                job.RevisionDate = turkeyTime;
                job.RevisionAssignedBy = userId;

                // 🔥 YENİ: Revize geçmişine kaydet (worker kendisi revizeye aldı)
                var revisionHistory = new JobRevisionHistory
                {
                    JobId = jobId,
                    AssignedBy = userId,
                    AssignedByName = "Worker (Kendisi)",
                    RevisionReason = dto.Note,
                    RevisionDate = turkeyTime,
                    Status = "Active"
                };
                _context.JobRevisionHistories.Add(revisionHistory);
            }
            else if (dto.Status == 2 && !string.IsNullOrWhiteSpace(dto.Note))
            {
                // Tamamlandı → Completed
                job.Status = "Completed";

                // 🔥 YENİ: Aktif revizeleri tamamla
                var activeRevisions = await _context.JobRevisionHistories
                    .Where(h => h.JobId == jobId && h.Status == "Active")
                    .ToListAsync();

                foreach (var rev in activeRevisions)
                {
                    rev.Status = "Completed";
                    rev.CompletedDate = turkeyTime;
                }
            }



            // 3️⃣ Saha raporu oluştur
            var report = new JobFieldReport
            {
                JobId = jobId,
                UserId = userId,
                FieldStatus = dto.Status,
                Note = dto.Note,
                CreatedAt = turkeyTime // Türkiye saati
            };

            _context.JobFieldReports.Add(report);

            // 4️⃣ FieldStatus = 2 (Tamamlandı) VE NOT varsa iş durumunu güncelle
            if (dto.Status == 2 && !string.IsNullOrWhiteSpace(dto.Note))
            {
                job.Status = "Completed"; // İş "Biten İşler"e taşınır
            }

            // 5️⃣ Veritabanına kaydet
            await _context.SaveChangesAsync();

            return Ok(report);
        }

        // GET /api/jobs/{jobId}/field-report
        // İşin en son kaydedilen saha raporunu getirir
        [HttpGet("{jobId}/field-report")]
        public async Task<IActionResult> GetLatestReport(int jobId)
        {
            // İş kaydı var mı kontrol et
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null)
                return NotFound("İş kaydı bulunamadı");

            // En son kaydedilen raporu çek
            var report = await _context.JobFieldReports
                .Where(r => r.JobId == jobId)
                .OrderByDescending(r => r.CreatedAt)
                .FirstOrDefaultAsync();

            if (report == null)
                return NotFound("Rapor bulunamadı");

            return Ok(report);
        }
    }

   


}
