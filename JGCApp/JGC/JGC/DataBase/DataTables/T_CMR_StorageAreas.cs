using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_CMR_StorageAreas")]
    public class T_CMR_StorageAreas
    {
        public int Project_ID { get; set; }
        public string Job_Code_Key { get; set; }        
        public string Store_Location { get; set; }
        public string Storage_Area { get; set; }
        public string PJ_Commodity { get; set; }
        public string Sub_Commodity { get; set; }
        public string Size_Descr { get; set; }
        public double Avail_Stock_Qty { get; set; }
    }
}
