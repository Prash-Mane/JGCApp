using JGC.Models.Completions;
using Newtonsoft.Json;
using SQLite;
using System.Collections.Generic;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_TAG")]
    public class T_TAG
    {
        public string system { get; set; }
        public string subSystem { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public string description { get; set; }
        public string refType { get; set; }
        public string jobPack { get; set; }
        public string workpack { get; set; }
        public string discipline { get; set; }
        public string SheetName { get; set; }
        public string CCMSHeaderID { get; set; }
        public string CCMSMasterID { get; set; }
        public string pcwbs { get; set; }
        public string fwbs { get; set; }
        public string drawing { get; set; }
        public string drawingRevision { get; set; }
        public string tagClass { get; set; }
        public string tagCategory { get; set; }
        public string tagFunction { get; set; }
        [Ignore]
        public string StatusColor { get; set; }
        public string id { get; set; }       
        public string ProjectName { get; set; }
    }
}
