using Newtonsoft.Json;
using SQLite;
using System;


namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_SignOffHeader")]
    public class T_SignOffHeader
    {
        [PrimaryKey, AutoIncrement]
        public int? rowid { get; set; }
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

    }
}
