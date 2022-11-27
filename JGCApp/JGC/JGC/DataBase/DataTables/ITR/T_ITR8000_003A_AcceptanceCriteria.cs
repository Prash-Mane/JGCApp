using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITRRecords_8000_003A_AcceptanceCriteria")]
   public class T_ITR8000_003A_AcceptanceCriteria
    {
        [PrimaryKey]
        public long RowID { get; set; }
        public long CommonRowID { get; set; }
        public long ID { get; set; }
        public long ITRCommonID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public string LECEText { get; set; }
        public string LECE { get; set; }
        public string LLCCText { get; set; }        
        public string LLCC { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public List<string> LECEOptionsList { get; set; }
        [Ignore]
        public List<string> LLCCOptionsList { get; set; }
        [Ignore]
        public bool IsReqLECEText { get; set; }
        [Ignore]
        public bool IsReqLLCCText { get; set; }
        [Ignore]
        public bool IsReqLECE { get; set; }
        [Ignore]
        public bool IsReqLLCC { get; set; }
        [Ignore]
        public bool IsReadyOnlyLLCCText { get; set; }
    }
}
