using System;
using System.Collections.Generic;
using System.Text;

namespace EFModels.MainDB
{
    public class System
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<BP> BPs { get; set; }
    }
}
