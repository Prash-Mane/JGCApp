using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.WorkPack
{
    [Table("T_IWPCompanyCategoryCodes")]
    public class T_IWPCompanyCategoryCodes
    {
        public long ProjectID { get; set; }
        public string CompanyCategoryCode { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return CompanyCategoryCode + " - " + Description;
        }
    }
}
