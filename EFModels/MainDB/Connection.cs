using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFModels.MainDB
{
    public class Connection
    {
        [Key]
        public long Id { get; set; }
        public string System { get; set; }
        public string ConnectionString { get; set; }
        public string Source { get; set; }
        public string SelectionCondition { get; set; }
        
        public List<BpStructure> BpStructures { get; set; }
    }
}
