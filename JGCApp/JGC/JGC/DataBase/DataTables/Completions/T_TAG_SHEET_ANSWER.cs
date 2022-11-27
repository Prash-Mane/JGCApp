using SQLite;
using System;
using System.Collections.Generic;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_TAG_SHEET_ANSWER")]
    public class T_TAG_SHEET_ANSWER
    {
        public string checkSheetName { get; set; }
        public string tagName { get; set; }
        public string jobPack { get; set; }
        public string ccmsHeaderId { get; set; }

        public string itemno { get; set; }
        public string checkValue { get; set; }
        public string isChecked { get; set; }
        public DateTime isDate { get; set; }
        public string completedBy { get; set; }
        public DateTime completedOn { get; set; }
        public bool IsSynced { get; set; }
        public string ProjectName { get; set; }

    }
}
