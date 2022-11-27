using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_TestRecordImage")]
    public class T_TestRecordImage
    {
        [PrimaryKey, Indexed]
        public int ID { get; set; }
        public long ProjectID { get; set; }
        public int ETestPackageID { get; set; }
        public string DisplayName { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public int FileSize { get; set; }
        public string FileBytes { get; set; }
    }
}
