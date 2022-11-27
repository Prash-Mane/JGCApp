using JGC.Models.Completions;
using SQLite;
using System.Collections.Generic;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_CHECKSHEET")]
    public class T_CHECKSHEET
    {
        public string name { get; set; }
        public string description { get; set; }
        public int ItrCategory { get; set; }
        public string ProjectName { get; set; }
        [Ignore]
        public bool ccsItr { get; set; }
        [Ignore]
        public string workpackName { get; set; }
        [Ignore]
        public List<CheckSheetAnswerModel> ItrAnswerList { get; set; }

        [Ignore]
        public ITRs ITRData { get; set; }


    }
}
