using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_AdminControlLogNaAutoSignatures")]
    public class T_AdminControlLogNaAutoSignatures
    {
        public long ProjectID { get; set; }
        public long ControlLogAdminID { get; set; }
        public long AutoSignOffControlLogAdminId { get; set; }
    }
}
