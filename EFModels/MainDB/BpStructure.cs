using EFModels.LogsDB;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFModels.MainDB
{
    public class BpStructure
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }


        //TODO: либо json (как набор пар:название поля, тип; или конкретный класс) либо просто название таблицы для сохранения логов
        [Column(TypeName ="jsonb")]
        public EventLog Structure { get; set; }
        
        
        public long ConnectionId { get; set; }
        
        public Connection Connection { get; set; }
    }
}
