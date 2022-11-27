using SQLite;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_Handover")]
    public class T_Handover
    {
       
        public string filedate { get; set; }
        public string type { get; set; }
        public string testpackname { get; set; }
        public int number { get; set; }
        public string name { get; set; }
        public string system { get; set; }
        public string subsystem { get; set; }
       // public object type { get; set; }
        public bool testpack { get; set; }
        public string ProjectName { get; set; }
    }
}
