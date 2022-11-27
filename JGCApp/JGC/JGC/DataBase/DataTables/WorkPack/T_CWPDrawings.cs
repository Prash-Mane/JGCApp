using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.WorkPack
{
    [Table("T_CWPDrawings")]
    public class T_CWPDrawings
    {
        public override string ToString()
        {
            return DisplayName;
        }
        public int Project_ID { get; set; }
        public int IWPID { get; set; }
        public int VMHubID { get; set; }
        public int CWPID { get; set; }
        public string DisplayName { get; set; }
        public string FileBytes { get; set; }
        public string Extension { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }

    }
}
