using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_WorkPack")]
    public class T_WorkPack
    {
        public string name { get; set; }
        public string location { get; set; }
        public string description { get; set; }
        public List<T_TAG> tags { get; set; }
        public List<T_CHECKSHEET> checkSheets { get; set; }
        public Dictionary<string, List<string>> tagNameToSheetNameMap { get; set; }
    }
}

