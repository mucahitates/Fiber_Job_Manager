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
            // 1️⃣ Bu job gerçekten var mı diye kontrol ediyoruz
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null)
                return NotFound("Job not found");

            // 2️⃣ Şu an giriş yapmış kullanıcının ID’sini alıyoruz
            // (Login sonrası JWT veya Claim içinde saklı olmalı)
            var userId = int.Parse(User.FindFirst("userId").Value);

            // 3️⃣ Veritabanına yazılacak saha raporu objesini oluşturuyoruz
            var report = new JobFieldReport
            {
                JobId = jobId,            // Hangi proje
                UserId = userId,          // Kim yazdı
                FieldStatus = dto.Status,// ComboBox’tan gelen saha durumu
                Note = dto.Note,          // Popup’taki açıklama
                CreatedAt = DateTime.UtcNow // Ne zaman yazıldı
            };

            // 4️⃣ Entity’yi EF Core'a ekliyoruz
            _context.JobFieldReports.Add(report);

            // 5️⃣ Veritabanına fiziksel olarak kaydediyoruz
            await _context.SaveChangesAsync();

            // 6️⃣ Client’a kaydedilen objeyi geri dönüyoruz
            return Ok(report);
        }
    }

    // UI’den gelen JSON body buraya map edilir
    public class FieldReportDto
    {
        public int Status { get; set; }  // 0 = yapılmadı, 1 = yapılamıyor, 2 = tamamlandı
        public string Note { get; set; } // Kullanıcının yazdığı açıklama
    }
}
