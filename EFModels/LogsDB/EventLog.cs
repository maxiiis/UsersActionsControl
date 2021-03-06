﻿using System;
using System.ComponentModel.DataAnnotations;

namespace EFModels.LogsDB
{
    public class EventLog
    {
        [Key]
        public long EventId { get; set; }
        public long CaseId { get; set; }
        public string ResourceId { get; set; }
        public User Resource { get; set; }
        public string ActivityId { get; set; }
        public bool AnalysStatus { get; set; }
        public DateTime TimeStamp { get; set; }
        public int Cost { get; set; }

        public Activity Activity { get; set; }
    }
}
