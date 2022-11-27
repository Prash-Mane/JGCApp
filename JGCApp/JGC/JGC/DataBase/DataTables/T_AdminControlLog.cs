using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_AdminControlLog")]
    public class T_AdminControlLog
    {
        [PrimaryKey, Indexed]
        public int ID { get; set; }
        public long ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string FunctionCode { get; set; }
        public string CompanyCategoryCode { get; set; }
        public string SignatureName { get; set; }
        public int SignatureNo { get; set; }
        public string DisciplineDisplay { get; set; }
        public string SectionCode { get; set; }
        public bool Milestone { get; set; }
        public int AssociatedSignatureNo { get; set; }
        public bool PunchesCompleted { get; set; }
        public bool PunchesConfirmed { get; set; }  
        [Ignore]
        public List<int> Folder { get; set; }
        [Ignore]
        public List<string> PunchCategory { get; set; }
        [Ignore]
        public List<int> PunchLayer { get; set; }
        [Ignore]
        public List<string> PunchActionParty { get; set; }
        [Ignore]
        public List<int> NAAutoSignOffControlLogID { get; set; }
    }
}
