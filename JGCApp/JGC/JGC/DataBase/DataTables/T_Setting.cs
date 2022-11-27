using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_Setting")]
    public class T_Setting
    {
        [PrimaryKey, Indexed]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string Error { get; set; }
    }
}
