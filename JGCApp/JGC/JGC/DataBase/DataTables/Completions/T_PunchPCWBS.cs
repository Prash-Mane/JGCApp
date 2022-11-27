using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_PunchPCWBS")]
    public class T_PunchPCWBS
    {
        public string pcwbs { get; set; }
        public string Jobcode { get; set; }
        public string ModelName { get; set; }
    }
}
