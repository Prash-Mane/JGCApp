using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.WorkPack
{
    [Table("T_CwpTag")]
  public  class T_CwpTag
    {
        public string ID { get; set; }
        public string TagNo { get; set; }
        public int ProjectID { get; set; }
        public int IWPID { get; set; }

    }
}
