using JGC.Models;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_EReports_UsersAssigned")]
    public class T_EReports_UsersAssigned 
    {
        public long RowID { get; set; }
        public int EReportID { get; set; }
        public int UserID { get; set; }
        public int SignatureRulesID { get; set; }
        public bool CanEdit { get; set; }
        public string Error { get; set; }

    }
}
