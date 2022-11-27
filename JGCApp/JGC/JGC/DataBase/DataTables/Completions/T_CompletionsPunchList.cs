using Newtonsoft.Json;
using SQLite;
using System;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_CompletionsPunchList")]
   public class T_CompletionsPunchList
    {
        [PrimaryKey, AutoIncrement]
        public int? rowid { get; set; }
        public int id { get; set; }
        public int itemnNo { get; set; }
        public string originator { get; set; }
        public DateTime ordate { get; set; }
        public string location { get; set; }
        public string systemno { get; set; }
        public string milestone { get; set; }
        public string tagno { get; set; }
        public string pandid { get; set; }
        public string description { get; set; }
        public string priority { get; set; }
        public string respdisc { get; set; }
        public string rfqreq { get; set; }
        public string status { get; set; }
        public string originatordisc { get; set; }
        public int msorder { get; set; }
        public int originatoruserid { get; set; }
        public string project { get; set; }
        public string itrname { get; set; }
        public string comments { get; set; }
        public bool synced { get; set; }
        public string uniqueno { get; set; }
        public string imageLocalLocation { get; set; }
        public string itrItemNo { get; set; }
        public string area { get; set; }
        public string subsystem { get; set; }
        public string workpack { get; set; }
        public string jobpack { get; set; }
        public string PunchDescriptionLevel1 { get; set; }
        public string PunchDescriptionLevel2 { get; set; }
        public string PunchDescriptionLevel3 { get; set; }
        public string RespPosition { get; set; }
        [JsonProperty("signOffby1")]
        public string COLUMN_PUNCH_signOffby1 { get; set; }
        [JsonProperty("signOffOn1")]
        public DateTime COLUMN_PUNCH_signOffOn1 { get; set; }
        [JsonProperty("signOffComment1")]
        public string COLUMN_PUNCH_signOffComment1 { get; set; }
        [JsonProperty("signOffID1")]
        public int COLUMN_PUNCH_signOffID1 { get; set; }
        [JsonProperty("signOffby2")]
        public string COLUMN_PUNCH_signOffby2 { get; set; }
        [JsonProperty("signOffOn2")]
        public DateTime COLUMN_PUNCH_signOffOn2 { get; set; }
        [JsonProperty("signOffComment2")]
        public string COLUMN_PUNCH_signOffComment2 { get; set; }
        [JsonProperty("signOffID2")]
        public int COLUMN_PUNCH_signOffID2 { get; set; }
        [JsonProperty("signOffby3")]
        public string COLUMN_PUNCH_signOffby3 { get; set; }
        [JsonProperty("signOffOn3")]
        public DateTime COLUMN_PUNCH_signOffOn3 { get; set; }
        [JsonProperty("signOffComment3")]
        public string COLUMN_PUNCH_signOffComment3 { get; set; }
        [JsonProperty("signOffID3")]
        public string COLUMN_PUNCH_signOffID3 { get; set; }
        public string PCWBS { get; set; }
        public string FWBS { get; set; }
        public string TCU { get; set; }
        public string PTU { get; set; }
        public string PunchType { get; set; }
        public string IssuerOwner { get; set; }
    }

}
