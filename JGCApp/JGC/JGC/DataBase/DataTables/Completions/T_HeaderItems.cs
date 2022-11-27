using SQLite;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_HeaderItems")]
   public class T_HeaderItems
    {
        public string modelName { get; set; }
        public string tagNum { get; set; }
        public string itr { get; set; }
        public string field { get; set; }
        public string value { get; set; }
    }
}
