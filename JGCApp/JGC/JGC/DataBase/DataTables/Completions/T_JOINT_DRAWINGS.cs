using SQLite;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_JOINT_DRAWINGS")]
   public class T_JOINT_DRAWINGS
    {
        public int _id { get; set; }
        public int COLUMN_ID { get; set; }
        public string COLUMN_DOCUMENT_TITLE { get; set; }
        public string COLUMN_FOLDER_STRUCTURE { get; set; }
        public string ProjectName { get; set; }
    }
}
