using SQLite;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_PunchListDropDowns")]
   public class T_PunchListDropDowns
    {
        public string system { get; set; }
        public string display { get; set; }
        public string value { get; set; }
        public string type { get; set; }
        public string id { get; set; }
    }
}
