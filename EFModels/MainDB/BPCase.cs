using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFModels.MainDB
{
    [Owned]
    public class BPCase
    {
        [Column(TypeName = "json")]
        public Case Case { get; set; }
        public long CaseId { get; set; }


    }
}
