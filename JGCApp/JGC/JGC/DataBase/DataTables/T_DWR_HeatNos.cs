using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_DWR_HeatNos")]
    public class T_DWR_HeatNos
    {
        public int Project_ID { get; set; }
        public string Ident_Code { get; set; }
        public string Heat_No { get; set; }
        public bool Updated { get; set; }
        public override string ToString()
        {
            return Heat_No;
        }
    }
}
