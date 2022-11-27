using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_AdminControlLogPunchLayer")]
    public class T_AdminControlLogPunchLayer
    {
        public long ProjectID { get; set; }
        public long ControlLogAdminID { get; set; }
        public int PunchAdminId { get; set; }
    }
}
