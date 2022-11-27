using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.WorkPack
{
    [Table("T_IWPAttachments")]
    public class T_IWPAttachments
    {
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public int LinkedID { get; set; }
        public string Module { get; set; }
        public string DisplayName { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public int FileSize { get; set; }
        public string FileBytes { get; set; }
    }
}
