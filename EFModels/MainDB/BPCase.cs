using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFModels.MainDB
{
    public class BPCase
    {
        [Key]
        public long CaseId { get; set; }
        //[Column(TypeName = "json")]
        //public Case Case { get; set; }

        public long BPId { get; set; }

        public BP BP { get; set; }

    }
}
