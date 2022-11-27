using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_AdminFolders")]
    public class T_AdminFolders
    {
        [PrimaryKey, Indexed]
        public int ID { get; set; }
        public long ProjectID { get; set; }
        public string FolderName { get; set; }
    }
}
