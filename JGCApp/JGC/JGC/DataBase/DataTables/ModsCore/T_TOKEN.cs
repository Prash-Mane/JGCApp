using SQLite;

namespace JGC.DataBase.DataTables.ModsCore
{
    [Table("T_TOKEN")]
    public class T_TOKEN
    {
        public string COLUMN_TOKEN_TOKEN { get; set; }
        public long COLUMN_TOKEN_TIME { get; set; }
    }
}
