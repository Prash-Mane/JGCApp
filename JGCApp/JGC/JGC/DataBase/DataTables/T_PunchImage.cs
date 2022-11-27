using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_PunchImage")]
    public class T_PunchImage
    {
        [PrimaryKey, Indexed]
        public int ID { get; set; }
        public long ProjectID { get; set; }
        public int ETestPackageID { get; set; }
        public string PunchID { get; set; }
        public string DisplayName { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public int FileSize { get; set; }
        public string FileBytes { get; set; }
        public bool Live { get; set; }
        public override string ToString()
        {
            return DisplayName.ToString();
        }
    }
}
