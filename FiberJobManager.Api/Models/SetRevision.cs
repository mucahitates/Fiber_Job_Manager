namespace FiberJobManager.Api.Models
{
    public class SetRevision
    {
        //  Revize nedeni
        public string RevisionReason { get; set; }

        //  Revize son tarihi
        public DateTime? RevisionDueDate
        {
            get; set;
        }
    }
}