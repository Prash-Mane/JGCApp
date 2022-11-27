using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_AdminControlLogFolder")]
    public class T_AdminControlLogFolder
    {
        public long ProjectID { get; set; }
        public long ControlLogAdminID { get; set; }
        public int FolderAdminID { get; set; }
    }
}
