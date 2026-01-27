namespace FiberJobManager.Api.Models
{
    public class JobRevisionHistory
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public int AssignedBy { get; set; }  // Revizeyi atan kişi
        public string AssignedByName { get; set; }  // Atayan kişinin adı
        public string RevisionReason { get; set; }
        public DateTime RevisionDate { get; set; }
        public DateTime? CompletedDate { get; set; }  // Revize tamamlanma tarihi
        public string Status { get; set; } = "Active";  // Active, Completed
    }
}