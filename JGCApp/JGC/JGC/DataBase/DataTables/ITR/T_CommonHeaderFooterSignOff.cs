using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_CommonHeaderFooterSignOff")]
    public class T_CommonHeaderFooterSignOff
    {
        public string SignOffTag { get; set; }
        public string SignOffChecksheet { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("signDate")]
        public DateTime SignDate { get; set; }

        [JsonProperty("rejected")]
        public bool Rejected { get; set; }

        [JsonProperty("rejectedReason")]
        public string RejectedReason { get; set; }
        public bool IsSynced { get; set; }
        public string ProjectName { get; set; }
        public string ModelName { get; set; }
        public long ITRCommonID { get; set; }
        public long CommonRowID { get; set; }
        public int CCMS_HEADERID { get; set; }
    }
}
