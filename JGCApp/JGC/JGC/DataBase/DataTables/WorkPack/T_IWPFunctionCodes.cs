using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.WorkPack
{
    [Table("T_IWPFunctionCodes")]
    public class T_IWPFunctionCodes
    {
        public long ProjectID { get; set; }
        public string FunctionCode { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return FunctionCode + " - " + Description;
        }
    }
}
