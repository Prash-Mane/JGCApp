using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_AdminControlLogPunchCategory")]
    public class T_AdminControlLogPunchCategory
    {
        public long ProjectID { get; set; }
        public long ControlLogAdminID { get; set; }
        public string Category { get; set; }
    }
}
