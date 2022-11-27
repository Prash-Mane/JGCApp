using SQLite;

namespace JGC.DataBase.DataTables.Completions
{
   [Table("T_CHECKSHEET_QUESTION")]
   public class T_CHECKSHEET_QUESTION
    {
        [PrimaryKey]
        public string id { get; set; }
        public string CheckSheet { get; set; }
        public string description { get; set; }
        public string section { get; set; }
        public string itemNo { get; set; }
        public string answerOptions { get; set; }
        public bool answerOptional { get; set; }
        public string ProjectName { get; set; }
    }
}
