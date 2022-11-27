using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.WorkPack
{
    [Table("T_IWPPunchCategory")]
    public class T_IWPPunchCategory
    {
        public int AdminControlLog_ID { get; set; }
        public int ProjectID { get; set; }
        public string PunchCategory { get; set; }
    }
}
