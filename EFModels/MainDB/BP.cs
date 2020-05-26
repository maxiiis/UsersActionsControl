using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFModels.MainDB
{
    public class BP
    {
        public long Id { get; set; }
        public long SystemId { get; set; }
        public string ConnectionString { get; set; }
        public string Source { get; set; }
        public string SelectionCondition { get; set; }
        public string Name { get; set; }
        //для динамической генерации
        public string Structure { get; set; }

        public System System { get; set; }
        public BPCase BPCase { get; set; }
    }
}
