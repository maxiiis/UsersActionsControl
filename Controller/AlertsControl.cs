using EFModels;
using EFModels.MainDB;
using System.Collections.ObjectModel;
using System.Linq;

namespace Controller
{
    public class AlertsControl
    {
        public AlertDTOs GetAlerts(AlertDTOs _tasks)
        {
            MainDBContext mainDB = new MainDBContext();
            var alerts = new Analyzer().FindAlerts();

            foreach (var a in alerts)
            {
                var task = new AlertDTO()
                {
                    БП = mainDB.BPs.FirstOrDefault(s => s.Id == a.BPId).Name,
                    Случай = a.CaseId,
                    Текст = a.Text,
                    Тип = a.Type
                };
                _tasks.Add(task);
            }
            return _tasks;
        }
    }

    public class AlertDTO
    {
        
        public string Тип { get; set; }
        public string БП { get; set; }
        public long Случай { get; set; }
        public string Текст { get; set; }

    }
    
    public class AlertDTOs : ObservableCollection<AlertDTO>
    {
        
    }
}
