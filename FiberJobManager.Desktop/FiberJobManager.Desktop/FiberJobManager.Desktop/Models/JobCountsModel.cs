using System.Text.Json.Serialization;

namespace FiberJobManager.Desktop.Models
{
    public class JobCountsModel
    {
        [JsonPropertyName("newJobs")]
        public int NewJobs { get; set; }

        [JsonPropertyName("revisionJobs")]
        public int RevisionJobs { get; set; }

        [JsonPropertyName("completedJobs")]
        public int CompletedJobs { get; set; }
    }
}