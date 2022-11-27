using SQLite;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_Tag_headers")]
    public class T_Tag_headers
    {
        public string tagId { get; set; }
        public string value { get; set; }
        public string display { get; set; }
        public string ProjectName { get; set; }
    }
}
