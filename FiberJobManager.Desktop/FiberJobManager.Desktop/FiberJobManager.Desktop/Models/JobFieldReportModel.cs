using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiberJobManager.Desktop.Models
{
    internal class JobFieldReportModel
    {

        public int Id { get; set; }
        public int JobId { get; set; }
        public int UserId { get; set; }
        public int FieldStatus { get; set; }
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
