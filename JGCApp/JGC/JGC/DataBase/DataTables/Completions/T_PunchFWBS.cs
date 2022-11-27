using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_PunchFWBS")]
    public class T_PunchFWBS
    {
        public string fwbs { get; set; }
        public string Jobcode { get; set; }
        public string ModelName { get; set; }
    }
}
