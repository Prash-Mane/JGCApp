using SQLite;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_HandoverWorkpacks")]
    public class T_HandoverWorkpacks
    {
        [PrimaryKey, AutoIncrement]
        public int? rowid { get; set; }
        public string COLUMN_HANDOVER_WPSYSTEM { get; set; }
        public string COLUMN_HANDOVER_WPSUBSYSTEM { get; set; }
        public string COLUMN_HANDOVER_WP { get; set; }
        public string ProjectName { get; set; }
    }
}
