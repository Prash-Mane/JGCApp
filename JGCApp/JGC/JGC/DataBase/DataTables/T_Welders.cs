using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_Welders")]
    public class T_Welders
    {
        public int Project_ID { get; set; }
        public string SubContractor { get; set; }
        public string Welder_Code { get; set; }
        public string Welder_Name { get; set; }
        public override string ToString()
        {
            return string.Format("{0} - {1}", Welder_Code, Welder_Name); ;
        }
    }
}
