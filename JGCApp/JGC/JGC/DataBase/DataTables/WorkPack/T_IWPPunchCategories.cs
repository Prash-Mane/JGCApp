using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.WorkPack
{
    [Table("T_IWPPunchCategories")]
    public class T_IWPPunchCategories
    {
        public long ProjectID { get; set; }
        public string Category { get; set; }
        public string ColourCode { get; set; }
        public bool SystemPunch { get; set; }
        public override string ToString()
        {
            return Category;
        }
    }
}
