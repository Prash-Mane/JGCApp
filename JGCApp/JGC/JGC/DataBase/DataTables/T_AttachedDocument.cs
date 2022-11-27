using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_AttachedDocument")]
    public class T_AttachedDocument
    {
        public long ProjectID { get; set; }
        public int ETestPackageID { get; set; }
        public string DisplayName { get; set; }
        public int FolderID { get; set; }
    }
}
