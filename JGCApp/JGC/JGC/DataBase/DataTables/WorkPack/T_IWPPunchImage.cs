using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.WorkPack
{
    [Table("T_IWPPunchImage")]
    public class T_IWPPunchImage
    {
        public long ProjectID { get; set; }
        public int IWPID { get; set; }
        public string Module { get; set; }
        public string LinkedID { get; set; }
        public string DisplayName { get; set; }
        public string FileName { get; set; }
        public string Comment { get; set; }
        public string Extension { get; set; }
        public string FileSize { get; set; }
        public string FileBytes { get; set; }
        public bool IsUploaded { get; set; }
        public override string ToString()
        {
            return DisplayName.ToString();
        }
    }
}
