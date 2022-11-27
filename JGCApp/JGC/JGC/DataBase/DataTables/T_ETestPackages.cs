using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_ETestPackages")]
    public class T_ETestPackages
    {
        [PrimaryKey, Indexed]
        public int ID { get; set; }
        public long ProjectID { get; set; }
        public string ModelName { get; set; }
        public string TestPackage { get; set; }
        public string SubContractor { get; set; }
        public string PCWBS { get; set; }
        public string ActionBy { get; set; }
        public string Status { get; set; }
        public bool Priority { get; set; }
        public string AFINo { get; set; }
        public string SystemNo { get; set; }
        public bool Updated { get; set; }
        public string TestMedia { get; set; }
        public string TestPressure { get; set; }
        public string TestRecordRemarks { get; set; }
        public string DrainRecordRemarks { get; set; }
        public DateTime DrainingDate { get; set; }
        public long UpdatedByUserID { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool AFINoUpdated { get; set; }
        public bool TestRecordRemarksUpdated { get; set; }
        public bool DrainRecordRemarksUpdated { get; set; }
            public bool DrainingDateUpdated { get; set; }
        public override string ToString()
        {
            return TestPackage.ToString();
        }

        public string Error { get; set; }
    }
}
