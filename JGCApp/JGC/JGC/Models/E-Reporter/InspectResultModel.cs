using JGC.DataBase.DataTables;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.E_Reporter
{
    public class InspectResultModel
    {
        public string VI { get; set; }
        public string DI { get; set; }
        public T_RT_Defects VI_Comment { get; set; }
        public string DI_Comment { get; set; }
        public string Remark { get; set; }
        public string JobCode { get; set; }
        public string DWRID { get; set; }
        public string RowID { get; set; }
        public List<T_RT_Defects> VICommentList { get; set; }
    }
}
