using JGC.Models;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_EReports_Signatures")]
    public class T_EReports_Signatures
    {
        public long RowID { get; set; }
        public int SignatureRulesID { get; set; }
        public int EReportID { get; set; }
        public int SignatureNo { get; set; }
        public string DisplaySignatureName { get; set; }
        public bool Signed { get; set; }
        public int SignedByUserID { get; set; }
        public string SignedByFullName { get; set; }
        public DateTime SignedOn { get; set; }
        public bool SendToDataHub { get; set; }
        public bool Updated { get; set; }
        public bool VMSigned { get; set; }
        public override string ToString()
        {
            return DisplaySignatureName;
        }
        public string Error { get; set; }
    }
}
