using SQLite;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_JOINT_QUESTIONS")]
    public class T_JOINT_QUESTIONS
    {
        public int COLUMN_QUESTION_JOINT_ID { get; set; }
        public int COLUMN_QUESTION_id { get; set; }
        public string COLUMN_QUESTION_description { get; set; }
        public string COLUMN_QUESTION_boolChecked { get; set; }
        public string COLUMN_QUESTION_itemNo { get; set; }
        public string COLUMN_QUESTION_textValue { get; set; }
        public string COLUMN_QUESTION_SYNCED { get; set; }
        public string COLUMN_QUESTION_CONTRACTOR { get; set; }
        public int COLUMN_QUESTION_CCTRID { get; set; }
        public string ProjectName { get; set; }
    }
}
