using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_CMR_HeatNos")]
    public class T_CMR_HeatNos
    {
        public int Project_ID { get; set; }
        public string Job_Code_Key { get; set; }
        public string Store_Location { get; set; }
        public string Heat_No { get; set; }
        public string PJ_Commodity { get; set; }
        public string Sub_Commodity { get; set; }
        public string Size_Descr { get; set; }
    }   
}
