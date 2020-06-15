using EFModels.MainDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Text;

namespace EFModels
{
    public class Alert
    {
        public long Id { get; set; }
        public long BPId { get; set; }
        public long CaseId { get; set; }
        public string Text { get; set; }

        public BP BP { get; set; }
        [ForeignKey("CaseId")]
        public BPCase BPCase { get; set; }
    }
}
