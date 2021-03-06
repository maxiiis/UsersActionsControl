﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFModels.MainDB
{
    public class BP
    {
        public long Id { get; set; }
        public long SystemId { get; set; }
        public string ConnectionString { get; set; }
        public string Source { get; set; }
        [Column(TypeName = "json")]
        public AccessMatrix AccessMatrix { get; set; }
        public string Name { get; set; }
        //для динамической генерации
        public string Structure { get; set; }
        [Column(TypeName = "json")]
        public StandartCase StandartCase { get; set; }
        public System System { get; set; }
        public List<BPCase> BPCases { get; set; }

        public List<Alert> Alerts { get; set; }
    }
}
