using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_AdminTestRecordDetails")]
    public class T_AdminTestRecordDetails
    {
        [PrimaryKey, Indexed]
        public int ID { get; set; }
        public long ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int OrderNo { get; set; }
        public string Description { get; set; }
    }
}
