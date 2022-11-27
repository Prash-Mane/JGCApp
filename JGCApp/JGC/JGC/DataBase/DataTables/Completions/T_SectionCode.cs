using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_SectionCode")]
    public class T_SectionCode
    {
        public string SectionCode { get; set; }
        public string Description { get; set; }
        public string ModelName { get; set; }
    }
}
