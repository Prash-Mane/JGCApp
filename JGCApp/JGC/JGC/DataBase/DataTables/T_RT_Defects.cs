using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{    
    [Table("T_RT_Defects")]
    public class T_RT_Defects
    {
        public string RT_Defect_Code { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return string.Format("{0} - {1}", RT_Defect_Code, Description);
        }
    }
}
