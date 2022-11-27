using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    
    [Table("T_AdminPostTestRecordAcceptedBy")]
    public class T_AdminPostTestRecordAcceptedBy
    {
        [PrimaryKey, Indexed]
        public int ID { get; set; }
        public long ProjectID { get; set; }
        public int OrderNo { get; set; }
        public string Description { get; set; }
        public string CompanyCategoryCode { get; set; }
        public string FunctionCode { get; set; }
        public bool ControlLogLinked { get; set; }
        public int ControlLogAdminID { get; set; }
    }
}
