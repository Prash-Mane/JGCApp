using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_AdminPunchCategories")]
    public class T_AdminPunchCategories
    {
        public long ProjectID { get; set; }
        public string Category { get; set; }
        public string ColourCode { get; set; }
        public bool SystemPunch { get; set; }
    }
}
