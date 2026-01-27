using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FiberJobManager.Desktop.Models
{
    internal class RevisionNoteModel
    {


        [JsonPropertyName("revisionReason")]
        public string RevisionReason { get; set; }  // Admin'in revize nedeni

        [JsonPropertyName("workerNote")]
        public string WorkerNote { get; set; }  // Worker'ın notu
    }
}
