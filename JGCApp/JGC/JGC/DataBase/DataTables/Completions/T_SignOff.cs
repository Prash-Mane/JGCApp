using SQLite;


namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_SignOff")]
    public class T_SignOff
    {
        public string modelName { get; set; }
        public string tagNum { get; set; }
        public string itr { get; set; }
        public string type { get; set; }
        public string signature { get; set; }
        public string date { get; set; }
        public string company { get; set; }
        public string client { get; set; }


    }
}
