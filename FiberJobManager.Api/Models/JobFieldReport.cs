namespace FiberJobManager.Api.Models
{

    public class JobFieldReport
    {

        // Dashboard/Yeni Projeler paneli verileri

        public int Id { get; set; }

        public int JobId { get; set; }
        public Job Job { get; set; }

        //log için işlem yapan bilgisi
        public int UserId { get; set; }
        public User User { get; set; }

        // 0 = Yapılmadı, 1 = Yapılamıyor, 2 = Tamamlandı
        public int FieldStatus { get; set; }


        //Pop-up açıklaması
        public string Note { get; set; }

        public DateTime CreatedAt { get; set; }
    }


}
