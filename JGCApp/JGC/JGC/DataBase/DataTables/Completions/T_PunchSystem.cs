using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_PunchSystem")]
    public class T_PunchSystem
    {
        public string SystemKey { get; set; }
        public string SystemValue { get; set; }
        public string Jobcode { get; set; }
        public string ModelName { get; set; }
        //public override string ToString()
        //{
        //    return SystemKey + " : " + SystemValue;
        //}
    }
}
