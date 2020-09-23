using EFModels.LogsDB;
using EFModels.MainDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Controller
{
    
    public class LogsControl
    {
        private MainDBContext mainDB = new MainDBContext();
        private LogDBContext logDB = new LogDBContext();

        private List<EventLog> LogsFromDB()
        {
            return logDB.EventLogs.Include(s => s.Activity).Include(s => s.Resource).ToList();
        }

        private List<BPCase> BPFromDB()
        {
            return mainDB.BPCases.Include(s => s.BP).Include(s => s.BP.System).ToList();
        }

        public LogDTOs GetLogs(LogDTOs _tasks)
        {
            var logsFromDb = LogsFromDB();
            var bpFromDb = BPFromDB();

            foreach (var s in logsFromDb)
            {
                var task = new LogDTO()
                {
                    Система = bpFromDb.FirstOrDefault(c => c.CaseId == s.CaseId).BP.System.Name,
                    БП = bpFromDb.FirstOrDefault(c => c.CaseId == s.CaseId).BP.Name,
                    Случай = bpFromDb.FirstOrDefault(c => c.CaseId == s.CaseId).CaseId,
                    Время = s.TimeStamp.ToShortDateString(),
                    Этап = s.ActivityId,
                    ТекстЭтапа = s.Activity.StatusText
                };

                if (s.Resource != null)
                {
                    task.Исполнитель = s.ResourceId.ToString();
                    task.Филиал = s.Resource.Filial;
                    task.Департамент = s.Resource.Department;
                }

                _tasks.Add(task);
            }

            return _tasks;
        }

    }
    public class LogDTO
    {
        
        public string Система { get; set; }
        public string БП { get; set; }
        public long Случай { get; set; }
        public string Этап { get; set; }
        public string ТекстЭтапа { get; set; }
        public string Время { get; set; }
        public string Исполнитель { get; set; }
        public string Департамент { get; set; }
        public string Филиал { get; set; }

    }
    
    public class LogDTOs : ObservableCollection<LogDTO>
    {
        
    }
}


