using System;
using System.ComponentModel.DataAnnotations;

namespace EFModels.LogsDB
{
    public class EventLog
    {
        [Key]
        public long EventId { get; set; }
        public long CaseId { get; set; }
        public string Resourse { get; set; }
        public string Activity { get; set; }
        public bool AnalysStatus { get; set; }
        public DateTime TimeStamp { get; set; }
        public int Cost { get; set; }

        public EventLogData eventLogData { get; set; }
    }
}
