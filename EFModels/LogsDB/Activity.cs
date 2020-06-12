using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFModels.LogsDB
{
    public class Activity
    {
        [Key]
        public string Id { get; set; }
        public string ActivityText { get; set; }
        public string ActivityText2 { get; set; }
        public string StatusText { get; set; }
        public List<EventLog> EventLog { get; set; }
    }
}
