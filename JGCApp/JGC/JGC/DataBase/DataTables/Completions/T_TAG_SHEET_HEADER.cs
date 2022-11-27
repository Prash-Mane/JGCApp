using Newtonsoft.Json;
using SQLite;
using System.Collections.Generic;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_TAG_SHEET_HEADER")]
    public class T_TAG_SHEET_HEADER
    {
        public string TagName { get; set; }
        public string ChecksheetName { get; set; }
        public string ColumnKey { get; set; }
        public string ColumnValue { get; set; }
        public string JobPack { get; set; }
        public string ProjectName { get; set; }
        [Ignore]
        public bool IsDropdown{ get; set;}
        [Ignore]
        public bool IsLabel { get; set; }
        [Ignore]
        public List<string> DrawingNoList { get; set; }

    }
}
