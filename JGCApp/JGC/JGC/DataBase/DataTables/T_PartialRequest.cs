using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_PartialRequest")]
    public class T_PartialRequest
    {
        [PrimaryKey, Indexed]
        public int ID { get; set; }
        public int EReportID { get; set; }
        public DateTime RequestedOn { get; set; }
        public int RequestedByUserID { get; set; }
    }
}
