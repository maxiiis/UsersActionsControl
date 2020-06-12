using System.Collections.Generic;

namespace EFModels.LogsDB
{
    public class User
    {
        public string Id { get; set; }
        public string Filial { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get;set; }
        public string FirstName { get; set; }
        public string Department { get; set; }
        public List<EventLog> EventLogs { get; set; }
    }
}
