using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_AdminControlLogActionParty")]
    public class T_AdminControlLogActionParty
    {
        public long ProjectID { get; set; }
        public long ControlLogAdminID { get; set; }
        public string FunctionCode { get; set; }
    }
}
