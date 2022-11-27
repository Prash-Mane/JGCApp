using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_TestLimitDrawing")]
    public class T_TestLimitDrawing
    {
        [PrimaryKey, Indexed]
        public int ID { get; set; }
        public long ProjectID { get; set; }
        public int ETestPackageID { get; set; }
        public string DisplayName { get; set; }
        public string SpoolDrawingNo { get; set; }
        public string FileBytes { get; set; }
        public int OrderNo { get; set; }
        public bool IsSpoolDrawing { get; set; }
        public bool IsPID { get; set; }
        public string PCWBS { get; set; }
        public override string ToString()
        {
            return DisplayName.ToString();
        }
    }
}
