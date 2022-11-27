using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_CompanyCategoryCode")]
    public class T_CompanyCategoryCode
    {
        public string CompanyCategoryCode { get; set; }
        public string Description { get; set; }
        public string ModelName { get; set; }
    }
}
