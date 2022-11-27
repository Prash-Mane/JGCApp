using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_StorageAreas")]
    public class T_StorageAreas
    {
        public int Project_ID { get; set; }
        public string Store_Location { get; set; }
        public string Storage_Area_Code { get; set; }
        public string Sheet_No { get; set; }
    }
}
