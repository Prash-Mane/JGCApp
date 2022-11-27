using JGC.Models;
using SQLite;
using System;
using System.Collections.Generic;
using SQLiteNetExtensions.Attributes;
using System.Text;
using System.Collections.ObjectModel;

namespace JGC.DataBase.DataTables
{
    [Table("T_EReports")]
    public class T_EReports 
    {
       // [PrimaryKey, Indexed]
        public int ID { get; set; }
        public long RowID { get; set; }
        public string ModelName { get; set; }
        public string AFINo { get; set; }
        public string ReportNo { get; set; }
        public string ReportType { get; set; }
        public string PCWBS { get; set; }
        public string SubContractor { get; set; }
        public string JSONString { get; set; }
        public bool Priority { get; set; }
        public DateTime ReportDate { get; set; }
        public bool Updated { get; set; }
        [Ignore]
        public List<T_EReports_Signatures> Signatures { get; set; } 
        [Ignore]
        public List<T_EReports_UsersAssigned> UsersAssigned { get; set; }
        public string Error { get; set; }
    }
}
