using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_WPS_MSTR")]
    public class T_WPS_MSTR
    {
        public int Project_ID { get; set; }
        public string Wps_No { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return string.Format("{0} - {1}", Wps_No, Description);
        }
    }
}
