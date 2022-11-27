using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_TestRecordDetails")]
    public class T_TestRecordDetails
    {
        public long ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int ETestPackageID { get; set; }
        public int DetailsAdminID { get; set; }
        public string InputValue { get; set; }
        public DateTime Updated { get; set; }
        public string Error { get; set; }
    }
}
