using SQLite;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_TAG_HEADER")]
    public class T_TAG_HEADER
    {
        public string TAG_HEADER_COLUMN_TAG { get; set; }
        public string TAG_HEADER_COLUMN_KEY { get; set; }
        public string TAG_HEADER_COLUMN_VALUE { get; set; }
        public string ProjectName { get; set; }
    }
}
