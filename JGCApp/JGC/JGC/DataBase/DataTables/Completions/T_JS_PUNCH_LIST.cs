using SQLite;

namespace JGC.DataBase.DataTables.Completions
{

    [Table("T_JS_PUNCH_LIST")]
    public class T_JS_PUNCH_LIST
    {
        public int COLUMN_PUNCH_LINK_ID { get; set; }
        public int COLUMN_PUNCH_ID { get; set; }
        public string COLUMN_PUNCH_LINE_NO { get; set; }
        public string COLUMN_PUNCH_TEXT { get; set; }
        public string COLUMN_PUNCH_USERNAME { get; set; }
        public string COLUMN_PUNCH_SYSTEMNO { get; set; }
        public string COLUMN_PUNCH_CATA { get; set; }
        public string COLUMN_PUNCH_RFI { get; set; }
        public string COLUMS_PUNCH_SYNCED { get; set; }
        public string ProjectName { get; set; }
    }
}
