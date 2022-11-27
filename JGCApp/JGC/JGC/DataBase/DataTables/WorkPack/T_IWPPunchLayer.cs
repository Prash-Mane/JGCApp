using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.WorkPack
{
    [Table("T_IWPPunchLayer")]
    public class T_IWPPunchLayer
    {
        public int AdminControlLog_ID { get; set; }
        public int ProjectID { get; set; }
        public string PunchLayer { get; set; }
    }
}
