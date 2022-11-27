using JGC.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_Drawings")]
    public class T_Drawings 
    {
        public override string ToString()
        {
            string ThisName = "";

            if (Sheet_No != string.Empty)
                ThisName = Name.ToUpper() + " Page " + Sheet_No;
            else
                ThisName = Name.ToUpper();
            return ThisName;
        }
        public string JobCode { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Sheet_No { get; set; }
        public string Total_Sheets { get; set; }
        public string Revision { get; set; }
        public string BinaryCode { get; set; }
        public string FileLocation { get; set; }
        public int EReportID { get; set; }
        public long RowID { get; set; }
        public string Error { get; set; }

    }
}
