using SQLite;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_SYSTEM")]
    public class T_SYSTEM
    {
        public string SYSTEM_COLUMN_NAME { get; set; }
        public string SYSTEM_COLUMN_LOCATION { get; set; }
        public string SYSTEM_COLUMN_DESCRIPTION { get; set; }
        public string ProjectName { get; set; }
    }
}
