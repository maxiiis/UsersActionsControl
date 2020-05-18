using Microsoft.EntityFrameworkCore;

namespace EFModels.LogsDB
{
    [Owned]
    public class EventLogData
    {
        public long EventId { get; set; }
        public string ResourseFIO { get; set; }
        public string ResourseDepartment { get; set; }
        public string ResourseFilial { get; set; }
        public string ActivityText { get; set; }
        public string ActivityText2 { get; set; }
        public EventLog eventLog { get; set; }
    }
}
