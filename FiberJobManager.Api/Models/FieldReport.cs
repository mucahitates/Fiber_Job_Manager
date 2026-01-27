namespace FiberJobManager.Api.Models
{
    public class FieldReportDto
    {
        public int Status { get; set; }  // 0 = yapılmadı, 1 = yapılamıyor, 2 = tamamlandı
        public string Note { get; set; } // Kullanıcının yazdığı açıklama
    }
}