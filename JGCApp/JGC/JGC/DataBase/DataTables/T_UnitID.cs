using System;
using SQLite;

namespace JGC.DataBase.DataTables
{
    [Table("T_UnitID")]
    public class T_UnitID
    {
        [PrimaryKey, Indexed]
        public int ID { get; set; }
        public string DeviceID { get; set; }
    }
}
