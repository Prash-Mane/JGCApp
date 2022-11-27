using SQLite;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_CHECKSHEET_PER_TAG")]
   public class T_CHECKSHEET_PER_TAG
    {
        public string TAGNAME { get; set; }
        public string CHECKSHEETNAME { get; set; }
        public string HEADER_ID { get; set; }
        public string JOBPACK { get; set; }
        public string ProjectName { get; set; }
        public bool ccsItr { get; set; }
        [Ignore]
        public string StatusColor { get; set; }
        [Ignore]
        public string description { get; set; }
    }
}
